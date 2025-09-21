using System.Data;
using CityCode.MandateSystem.Application.Common.Interfaces;
using CityCode.MandateSystem.Application.Dtos;
using CityCode.MandateSystem.Application.Extentions;
using CityCode.MandateSystem.Application.Services.UtilityServices.Interfaces;
using CityCode.MandateSystem.Application.Settings;
using CityCode.MandateSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Npgsql;

namespace CityCode.MandateSystem.WorkerService
{
    public class TransactionPostingService(
        ILogger<TransactionPostingService> logger,
        IServiceScopeFactory factory,
        IMandateService mandateService,
        IOptions<SystemSettings> systemSettings)
        : BackgroundService
    {
        // singleton, safe to inject directly
        private readonly SystemSettings _systemSettings = systemSettings.Value;
        private IApplicationDbContext _context = null!;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Worker started at: {Time}", DateTimeOffset.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = factory.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
                    _context = context;
                    logger.LogInformation("TRANSACTION POSTING WORKER RUNNING");

                    var today = DateOnly.FromDateTime(DateTime.Now);

                    var schedules = await context.MandateSchedules
                        .Include(s => s.Mandate)
                        .Include(s => s.MandateTransactions)
                        .Where(s =>
                            s.NextRunDate == today &&
                            !s.IsEnded &&
                            s.WorkflowStatus == WorkflowStatus.MANDATE_APPROVED_BY_BANK &&
                            !s.MandateTransactions.Any(t =>
                                t.MandateScheduleId == s.MandateScheduleId &&
                                t.TransactionDate == today))
                        .ToListAsync(stoppingToken);


                    foreach (var mandateSchedule in schedules)
                    {
                        logger.LogInformation("POSTING TRANSACTION FOR {MandateMandateId}", mandateSchedule.MandateId);

                        var result = await ExecuteFundsTransferWithRetry(mandateSchedule);

                        logger.LogInformation("POSTING TRANSACTION RESPONSE FOR {MandateMandateId}: {response}",
                            mandateSchedule.MandateId, JsonConvert.SerializeObject(result));

                        var transaction = new MandateTransaction(
                            mandateScheduleId: mandateSchedule.MandateScheduleId,
                            transactionReference: result.PaymentReference ?? string.Empty,
                            amount: result.Amount,
                            currency: "NGN",
                            transactionDate: today,
                            transactionStatus: result.ResponseCode == "00"
                                ? nameof(TransactionStatus.SUCCESSFUL)
                                : nameof(TransactionStatus.FAILED), mandateSchedule.MandateId);
                        transaction.UpdateFromResponse(result.ResponseCode, result.SessionID, result.ChannelCode,
                            result.NameEnquiryRef, result.DestinationInstitutionCode, result.BeneficiaryAccountName,
                            result.BeneficiaryAccountNumber, result.BeneficiaryKYCLevel,
                            result.BeneficiaryBankVerificationNumber,
                            result.OriginatorAccountName, result.OriginatorAccountNumber,
                            result.OriginatorBankVerificationNumber, result.OriginatorKYCLevel,
                            result.TransactionLocation, result.Narration, result.PaymentReference!);
                        transaction.UpdateStatus(transaction.TransactionStatus, "SUCCESSFUL", result.TransactionId);

                        context.MandateTransactions.Add(transaction);
                        await context.SaveChangesAsync(stoppingToken);

                        logger.LogInformation("DONE POSTING TRANSACTION FOR {MandateMandateId}",
                            mandateSchedule.MandateId);
                    }

                    logger.LogInformation("Worker completed cycle at: {Time}", DateTimeOffset.Now);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    logger.LogError("Exception while posting transaction {error}", e.Message);
                }

                await Task.Delay(TimeSpan.FromMinutes(4), stoppingToken);
            }
        }

        private async Task<MandateTransactionResponse> ExecuteFundsTransferWithRetry(MandateSchedule mandateSchedule)
        {
            const int fundsTransferMaxAttempts = 3;
            const int fundsTransferDelayMs = 2000; // 2 seconds
            var mandate = mandateSchedule.Mandate;
            ObjectExtentions.MandateTransactionPayload manadatetransactionPayload = null!;

            Exception? lastException = null;
            string nibbsFailureMessage = string.Empty;

            for (int attempt = 1; attempt <= fundsTransferMaxAttempts; attempt++)
            {
                try
                {
                    logger.LogInformation("Funds transfer attempt {Attempt}/{MaxAttempts} for mandate {MandateId}",
                        attempt, fundsTransferMaxAttempts, mandate.MandateId);

                    manadatetransactionPayload = mandate.BuildMandateTransactionPayload(_systemSettings.BankCode);
                    var result = await mandateService.DoFundsTransfer(mandate, manadatetransactionPayload);

                    logger.LogInformation("Funds transfer successful on attempt {Attempt} for mandate {MandateId}",
                        attempt, mandate.MandateId);

                    return result;
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    nibbsFailureMessage = ex.Message;
                    logger.LogWarning(
                        "Funds transfer attempt {Attempt}/{MaxAttempts} failed for mandate {MandateId}: {Error}",
                        attempt, fundsTransferMaxAttempts, mandate.MandateId, ex.Message);

                    // Don't delay after the last attempt
                    if (attempt < fundsTransferMaxAttempts)
                    {
                        logger.LogInformation("Retrying funds transfer for mandate {MandateId} in {Delay}ms...",
                            mandate.MandateId, fundsTransferDelayMs);
                        await Task.Delay(fundsTransferDelayMs);
                    }
                }
            }

            logger.LogError(
                "All {MaxAttempts} funds transfer attempts failed for mandate {MandateId}. Final error: {Error}",
                fundsTransferMaxAttempts, mandate.MandateId, lastException?.Message);


            var transaction = new MandateTransaction(
                mandateScheduleId: mandateSchedule.MandateScheduleId,
                transactionReference: string.Empty,
                amount: 0m,
                currency: "NGN",
                transactionDate: DateOnly.FromDateTime(DateTime.UtcNow),
                transactionStatus: nameof(TransactionStatus.FAILED), mandate.MandateId);
            transaction.UpdateFromResponse(String.Empty, string.Empty,
                int.Parse(manadatetransactionPayload.ChannelCode!),
                manadatetransactionPayload.NameEnquiryRef!, manadatetransactionPayload.DestinationInstitutionCode!,
                manadatetransactionPayload.BeneficiaryAccountName!,
                manadatetransactionPayload.BeneficiaryAccountNumber!, manadatetransactionPayload.BeneficiaryKYCLevel!,
                manadatetransactionPayload.BeneficiaryBankVerificationNumber!,
                manadatetransactionPayload.OriginatorAccountName!, manadatetransactionPayload.OriginatorAccountNumber!,
                manadatetransactionPayload.OriginatorBankVerificationNumber!,
                manadatetransactionPayload.OriginatorKYCLevel!, manadatetransactionPayload.TransactionLocation!,
                String.Empty, manadatetransactionPayload.PaymentReference!);
            transaction.UpdateStatus(transaction.TransactionStatus, nibbsFailureMessage,
                manadatetransactionPayload.TransactionId);

            _context.MandateTransactions.Add(transaction);
            await _context.SaveChangesAsync(CancellationToken.None);


            throw new InvalidOperationException(
                $"Funds transfer failed after {fundsTransferMaxAttempts} attempts for mandate {mandate.MandateId}: {lastException?.Message}",
                lastException);
        }
    }
}