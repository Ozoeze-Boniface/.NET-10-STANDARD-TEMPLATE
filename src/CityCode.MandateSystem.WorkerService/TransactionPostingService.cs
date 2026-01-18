using System.Globalization;
using CityCode.MandateSystem.Application.Common.Interfaces;
using CityCode.MandateSystem.Application.Dtos;
using CityCode.MandateSystem.Application.Extentions;
using CityCode.MandateSystem.Application.Services.UtilityServices.Interfaces;
using CityCode.MandateSystem.Application.Settings;
using CityCode.MandateSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CityCode.MandateSystem.WorkerService
{
    public class TransactionPostingService(
        ILogger<TransactionPostingService> logger,
        IServiceScopeFactory factory,
        IMandateService mandateService,
        IEmailService emailService,
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

                    var schedules = await _context.MandateSchedules
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

                        if (result.ResponseCode == "00")
                        {
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

                            await _context.MandateTransactions.AddAsync(transaction, stoppingToken);

                            if (mandateSchedule.Mandate.TakeCharge)
                            {
                                var resultForCharge = await ExecuteFundsTransferWithRetry(mandateSchedule,
                                    GetOnePercent(mandateSchedule.Mandate.TransactionAmount));

                                logger.LogInformation(
                                    "POSTING CHARGE TRANSACTION RESPONSE FOR {MandateMandateId}: {response}",
                                    mandateSchedule.MandateId, JsonConvert.SerializeObject(resultForCharge));

                                if (resultForCharge.ResponseCode == "00")
                                {
                                    var chargeTransaction = new MandateTransaction(
                                        mandateScheduleId: mandateSchedule.MandateScheduleId,
                                        transactionReference: result.PaymentReference ?? string.Empty,
                                        amount: result.Amount,
                                        currency: "NGN",
                                        transactionDate: today,
                                        transactionStatus: result.ResponseCode == "00"
                                            ? nameof(TransactionStatus.SUCCESSFUL)
                                            : nameof(TransactionStatus.FAILED), mandateSchedule.MandateId);
                                    chargeTransaction.UpdateFromResponse(result.ResponseCode, result.SessionID,
                                        result.ChannelCode,
                                        result.NameEnquiryRef, result.DestinationInstitutionCode,
                                        result.BeneficiaryAccountName,
                                        result.BeneficiaryAccountNumber, result.BeneficiaryKYCLevel,
                                        result.BeneficiaryBankVerificationNumber,
                                        result.OriginatorAccountName, result.OriginatorAccountNumber,
                                        result.OriginatorBankVerificationNumber, result.OriginatorKYCLevel,
                                        result.TransactionLocation, result.Narration, result.PaymentReference!);
                                    chargeTransaction.UpdateStatus(chargeTransaction.TransactionStatus, "SUCCESSFUL",
                                        result.TransactionId);

                                    await _context.MandateTransactions.AddAsync(chargeTransaction, stoppingToken);
                                }
                                else
                                {
                                    logger.LogError(
                                        "Charge Transaction failed for {MandateMandateId} with response code {ResponseCode}",
                                        mandateSchedule.MandateId, resultForCharge.ResponseCode);
                                }
                            }

                            mandateSchedule.UpdateToNextRunDate();
                            await _context.SaveChangesAsync(stoppingToken);

                            _ = Task.Run(async () =>
                            {
                                try
                                {
                                    var sent = await emailService.SendEmail(
                                        mandateSchedule.Mandate.PayerEmail,
                                        new MailContent
                                        {
                                            Header = "Direct Debit Notification",
                                            Subject = "CityCode Direct Debit Mandate Application",
                                            Body = $@"
                                                Dear {mandateSchedule.Mandate.PayerName},<br/><br/>

                                                This is to notify you that a <b>Direct Debit transaction</b> has been successfully processed on your account under your approved mandate with <b>CityCode</b>.<br/><br/>

                                                <b>Transaction Details:</b><br/>
                                                Amount: <b>NGN {result.Amount:N2}</b><br/>
                                                Transaction Reference: <b>{result.PaymentReference}</b><br/>
                                                Date: <b>{today:dd MMM yyyy}</b><br/>
                                                Status: <b>Successful</b><br/><br/>

                                                If this transaction was not authorized by you or you require further clarification, please contact our support team immediately.<br/><br/>

                                                Thank you for choosing CityCode.<br/><br/>

                                                Warm regards,<br/>
                                                <b>CityCode Direct Debit Team</b>
                                                "
                                        });

                                    if (sent)
                                    {
                                        logger.LogInformation(
                                            "Successfully sent mandate transaction notification to {PayerEmail} for MandateId {MandateId}",
                                            mandateSchedule.Mandate.PayerEmail,
                                            mandateSchedule.MandateId);
                                    }
                                    else
                                    {
                                        logger.LogWarning(
                                            "Failed to send mandate transaction notification to {PayerEmail} for MandateId {MandateId}",
                                            mandateSchedule.Mandate.PayerEmail,
                                            mandateSchedule.MandateId);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    logger.LogError(
                                        ex,
                                        "Error occurred while sending mandate transaction email to {PayerEmail} for MandateId {MandateId}",
                                        mandateSchedule.Mandate.PayerEmail,
                                        mandateSchedule.MandateId);
                                }
                            }, stoppingToken);

                            logger.LogInformation("DONE POSTING TRANSACTION FOR {MandateMandateId}",
                                mandateSchedule.MandateId);
                        }
                        else
                        {
                            logger.LogError(
                                "Transaction failed for {MandateMandateId} with response code {ResponseCode}",
                                mandateSchedule.MandateId, result.ResponseCode);
                        }
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

        private async Task<MandateTransactionResponse> ExecuteFundsTransferWithRetry(MandateSchedule mandateSchedule,
            decimal? amountOverride = 0)
        {
            const int fundsTransferMaxAttempts = 3;
            const int fundsTransferDelayMs = 2000; // 2 seconds
            var mandate = mandateSchedule.Mandate;
            ObjectExtentions.MandateTransactionPayload mandatetransactionPayload = null!;

            Exception? lastException = null;
            string nibbsFailureMessage = string.Empty;

            for (int attempt = 1; attempt <= fundsTransferMaxAttempts; attempt++)
            {
                try
                {
                    logger.LogInformation("Funds transfer attempt {Attempt}/{MaxAttempts} for mandate {MandateId}",
                        attempt, fundsTransferMaxAttempts, mandate.MandateId);

                    mandatetransactionPayload = mandate.BuildMandateTransactionPayload(_systemSettings.BankCode);
                    if (amountOverride is > 0)
                    {
                        mandatetransactionPayload.Amount = amountOverride.Value.ToString(CultureInfo.InvariantCulture);
                    }

                    var result = await mandateService.DoFundsTransfer(mandate, mandatetransactionPayload);

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
                amount: decimal.Parse(mandatetransactionPayload.Amount!),
                currency: "NGN",
                transactionDate: DateOnly.FromDateTime(DateTime.UtcNow),
                transactionStatus: nameof(TransactionStatus.FAILED), mandate.MandateId);
            transaction.UpdateFromResponse(String.Empty, string.Empty,
                int.Parse(mandatetransactionPayload.ChannelCode!),
                mandatetransactionPayload.NameEnquiryRef!, mandatetransactionPayload.DestinationInstitutionCode!,
                mandatetransactionPayload.BeneficiaryAccountName!,
                mandatetransactionPayload.BeneficiaryAccountNumber!, mandatetransactionPayload.BeneficiaryKYCLevel!,
                mandatetransactionPayload.BeneficiaryBankVerificationNumber!,
                mandatetransactionPayload.OriginatorAccountName!, mandatetransactionPayload.OriginatorAccountNumber!,
                mandatetransactionPayload.OriginatorBankVerificationNumber!,
                mandatetransactionPayload.OriginatorKYCLevel!, mandatetransactionPayload.TransactionLocation!,
                String.Empty, mandatetransactionPayload.PaymentReference!);
            transaction.UpdateStatus(transaction.TransactionStatus, nibbsFailureMessage,
                mandatetransactionPayload.TransactionId);

            mandateSchedule.UpdateToNextRunDate();

            var retryTransaction = new RetryTransaction(mandate.MandateId, false, string.Empty,
                nibbsFailureMessage, DateTime.UtcNow, decimal.Parse(mandatetransactionPayload.Amount!),
                mandateSchedule.NextRunDate, 1);

            _context.RetryTransactions.Add(retryTransaction);
            _context.MandateTransactions.Add(transaction);
            await _context.SaveChangesAsync(CancellationToken.None);


            throw new InvalidOperationException(
                $"Funds transfer failed after {fundsTransferMaxAttempts} attempts for mandate {mandate.MandateId}: {lastException?.Message}",
                lastException);
        }

        private decimal GetOnePercent(decimal amount)
        {
            decimal onePercent = amount * 0.01m;
            return onePercent > 1000 ? 1000 : onePercent;
        }
    }
}