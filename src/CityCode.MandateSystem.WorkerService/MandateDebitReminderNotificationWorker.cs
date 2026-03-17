using CityCode.MandateSystem.Application.Common.Interfaces;
using CityCode.MandateSystem.Application.Dtos;
using CityCode.MandateSystem.Application.Services.UtilityServices.Interfaces;
using CityCode.MandateSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CityCode.MandateSystem.WorkerService;

public class MandateDebitReminderNotificationWorker(
    ILogger<MandateDebitReminderNotificationWorker> logger,
    IServiceScopeFactory factory,
    IEmailService emailService)
    : BackgroundService
{
    private const int PageSize = 200;
    private readonly HashSet<string> _sentReminderKeys = [];
    private DateOnly _cacheDate = DateOnly.MinValue;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Mandate debit reminder worker started at: {Time}", DateTimeOffset.Now);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = factory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
                var today = DateOnly.FromDateTime(DateTime.Now);
                var tomorrow = today.AddDays(1);
                var twoDaysAway = today.AddDays(2);
                var lastSeenScheduleId = 0L;

                if (_cacheDate != today)
                {
                    _sentReminderKeys.Clear();
                    _cacheDate = today;
                }

                logger.LogInformation("MANDATE DEBIT REMINDER WORKER RUNNING");

                while (!stoppingToken.IsCancellationRequested)
                {
                    var schedules = await context.MandateSchedules
                        .Include(s => s.Mandate)
                        .Where(s =>
                            s.MandateScheduleId > lastSeenScheduleId &&
                            !s.IsEnded &&
                            s.WorkflowStatus == WorkflowStatus.MANDATE_APPROVED_BY_BANK &&
                            s.NextRunDate <= s.EndDate &&
                            (s.NextRunDate == tomorrow || s.NextRunDate == twoDaysAway))
                        .OrderBy(s => s.MandateScheduleId)
                        .Take(PageSize)
                        .ToListAsync(stoppingToken);

                    if (schedules.Count == 0)
                    {
                        break;
                    }

                    foreach (var mandateSchedule in schedules)
                    {
                        var daysToRunDate = mandateSchedule.NextRunDate.DayNumber - today.DayNumber;
                        if (daysToRunDate is not (1 or 2))
                        {
                            continue;
                        }

                        if (string.IsNullOrWhiteSpace(mandateSchedule.Mandate.PayerEmail))
                        {
                            logger.LogWarning(
                                "Skipping reminder for MandateId {MandateId} because payer email is empty.",
                                mandateSchedule.MandateId);
                            continue;
                        }

                        var reminderKey = $"{mandateSchedule.MandateScheduleId}:{mandateSchedule.NextRunDate:yyyyMMdd}:{daysToRunDate}";
                        if (!_sentReminderKeys.Add(reminderKey))
                        {
                            logger.LogDebug(
                                "Reminder already processed today for MandateScheduleId {MandateScheduleId}, NextRunDate {NextRunDate}, DaysToRunDate {DaysToRunDate}",
                                mandateSchedule.MandateScheduleId,
                                mandateSchedule.NextRunDate,
                                daysToRunDate);
                            continue;
                        }

                        try
                        {
                            var sent = await emailService.SendEmail(
                                mandateSchedule.Mandate.PayerEmail,
                                BuildReminderMailContent(mandateSchedule, daysToRunDate));

                            if (sent)
                            {
                                logger.LogInformation(
                                    "Successfully sent debit reminder ({DaysToRunDate}-day) to {PayerEmail} for MandateId {MandateId}",
                                    daysToRunDate,
                                    mandateSchedule.Mandate.PayerEmail,
                                    mandateSchedule.MandateId);
                            }
                            else
                            {
                                logger.LogWarning(
                                    "Failed to send debit reminder ({DaysToRunDate}-day) to {PayerEmail} for MandateId {MandateId}",
                                    daysToRunDate,
                                    mandateSchedule.Mandate.PayerEmail,
                                    mandateSchedule.MandateId);
                                _sentReminderKeys.Remove(reminderKey);
                            }
                        }
                        catch (Exception ex)
                        {
                            _sentReminderKeys.Remove(reminderKey);
                            logger.LogError(
                                ex,
                                "Error occurred while sending debit reminder ({DaysToRunDate}-day) to {PayerEmail} for MandateId {MandateId}",
                                daysToRunDate,
                                mandateSchedule.Mandate.PayerEmail,
                                mandateSchedule.MandateId);
                        }
                    }

                    lastSeenScheduleId = schedules[^1].MandateScheduleId;
                }

                logger.LogInformation("Mandate debit reminder worker cycle completed at: {Time}", DateTimeOffset.Now);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception while processing mandate debit reminders: {Error}", ex.Message);
            }

            await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
        }
    }

    private static MailContent BuildReminderMailContent(MandateSchedule mandateSchedule, int daysToRunDate)
    {
        var dayLabel = daysToRunDate == 1 ? "1 day" : "2 days";
        var runDate = mandateSchedule.NextRunDate.ToString("dd MMM yyyy");
        var amount = mandateSchedule.Mandate.TransactionAmount.ToString("N2");
        var subject = daysToRunDate == 1
            ? "Reminder: Your direct debit runs tomorrow"
            : "Reminder: Your direct debit runs in 2 days";

        return new MailContent
        {
            Header = "Upcoming Direct Debit Reminder",
            Subject = subject,
            Body = $@"
                Dear {mandateSchedule.Mandate.PayerName},<br/><br/>

                This is a reminder that your approved <b>CityCode Direct Debit</b> mandate will be processed in <b>{dayLabel}</b>.<br/><br/>

                <b>Debit Details:</b><br/>
                Amount: <b>NGN {amount}</b><br/>
                Mandate Reference: <b>{mandateSchedule.MandateReference}</b><br/>
                Scheduled Debit Date: <b>{runDate}</b><br/>
                Frequency: <b>{mandateSchedule.PaymentFrequency}</b><br/><br/>

                Please ensure your account is funded before the scheduled debit date to avoid failed transactions.<br/><br/>

                If you need support, contact the CityCode support team.<br/><br/>

                Warm regards,<br/>
                <b>CityCode Direct Debit Team</b>
                "
        };
    }
}
