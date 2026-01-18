using CityCode.MandateSystem.Application.Common.Exceptions;
using CityCode.MandateSystem.Application.Common.Interfaces;
using CityCode.MandateSystem.Application.Dtos;
using CityCode.MandateSystem.Application.Extentions;
using CityCode.MandateSystem.Application.Services.UtilityServices.Interfaces;
using CityCode.MandateSystem.Application.Settings;
using CityCode.MandateSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CityCode.MandateSystem.WorkerService;

public class RetryTransactionPostingService(
        ILogger<RetryTransactionPostingService> logger,
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
                logger.LogInformation("TRANSACTION RETRY POSTING WORKER RUNNING");

                var today = DateOnly.FromDateTime(DateTime.Now);

                var transactionRetries = await context.RetryTransactions.Where(t => !t.IsPosted && t.RetryCount < 8)
                    .ToListAsync(stoppingToken);


                foreach (var retry in transactionRetries)
                {
                    logger.LogInformation("POSTING RETRY TRANSACTION FOR {MandateId}", retry.MandateId);
                    var mandateSchedule = await context.MandateSchedules
                        .Include(s => s.Mandate)
                        .FirstOrDefaultAsync(s => s.MandateId == retry.MandateId, stoppingToken);

                    MandateTransactionResponse result = null!;
                    try
                    {
                        result = await ExecuteFundsTransferWithRetry(mandateSchedule!);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex.Message);
                        retry.RetryCount += 1;
                        await context.SaveChangesAsync(CancellationToken.None);
                        throw new BadRequestException(ex.Message);
                    }

                    logger.LogInformation("POSTING TRANSACTION RETRY FOR {MandateMandateId}: {response}",
                        retry.MandateId, JsonConvert.SerializeObject(result));

                    var transaction = new MandateTransaction(
                        mandateScheduleId: mandateSchedule!.MandateScheduleId,
                        transactionReference: result.PaymentReference,
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

                    // Update retry transaction as posted
                    retry.IsPosted = true;
                    retry.ResponseCode = result.ResponseCode;
                    retry.ResponseDescription = result.Narration;
                    retry.TransactionDate = DateTime.UtcNow;

                    mandateSchedule.UpdateToNextRunDate();
                    context.MandateTransactions.Add(transaction);
                    await context.SaveChangesAsync(stoppingToken);

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

                    logger.LogInformation("DONE POSTING TRANSACTION RETRY FOR {MandateMandateId}",
                        mandateSchedule.MandateId);
                }

                logger.LogInformation("Worker completed cycle at: {Time}", DateTimeOffset.Now);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                logger.LogError("Exception while posting transaction retry {error}", e.Message);
            }

            await Task.Delay(TimeSpan.FromHours(4), stoppingToken);
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
                logger.LogInformation("Funds transfer retry attempt {Attempt}/{MaxAttempts} for mandate {MandateId}",
                    attempt, fundsTransferMaxAttempts, mandate.MandateId);

                manadatetransactionPayload = mandate.BuildMandateTransactionPayload(_systemSettings.BankCode);
                var result = await mandateService.DoFundsTransfer(mandate, manadatetransactionPayload);

                logger.LogInformation("Funds transfer retry successful on attempt {Attempt} for mandate {MandateId}",
                    attempt, mandate.MandateId);

                return result;
            }
            catch (Exception ex)
            {
                lastException = ex;
                nibbsFailureMessage = ex.Message;
                logger.LogWarning(
                    "Funds transfer retry attempt {Attempt}/{MaxAttempts} failed for mandate {MandateId}: {Error}",
                    attempt, fundsTransferMaxAttempts, mandate.MandateId, ex.Message);

                // Don't delay after the last attempt
                if (attempt < fundsTransferMaxAttempts)
                {
                    logger.LogInformation("Retrying funds transfer retry for mandate {MandateId} in {Delay}ms...",
                        mandate.MandateId, fundsTransferDelayMs);
                    await Task.Delay(fundsTransferDelayMs);
                }
            }
        }

        logger.LogError(
            "All {MaxAttempts} funds transfer retry attempts failed for mandate {MandateId}. Final error: {Error}",
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
            $"Funds transfer retry failed after {fundsTransferMaxAttempts} attempts for mandate {mandate.MandateId}: {lastException?.Message}",
            lastException);
    }
}