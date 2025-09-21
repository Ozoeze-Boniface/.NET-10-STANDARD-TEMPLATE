using System;

namespace CityCode.MandateSystem.Domain.Entities;

public class MandateSchedule : BaseAuditableEntity
{
    public long MandateScheduleId { get; set; }
    public long MandateId { get; set; }
    public string MandateReference { get; set; } = default!;
    public string NibbsMandateCode { get; set; } = default!;
    public WorkflowStatus WorkflowStatus { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public DateOnly NextRunDate { get; set; }
    public PaymentFrequency PaymentFrequency { get; set; }
    public bool IsEnded { get; set; } = false;
    public DateTime? DateOfBankApproval { get; set; }
    public Mandate Mandate { get; set; } = default!;
    public ICollection<MandateTransaction> MandateTransactions { get; set; } = [];

    public MandateSchedule(long mandateId, string mandateReference, string nibbsMandateCode, WorkflowStatus workflowStatus, DateOnly startDate, DateOnly endDate, PaymentFrequency paymentFrequency)
    {
        MandateId = mandateId;
        MandateReference = mandateReference;
        NibbsMandateCode = nibbsMandateCode;
        WorkflowStatus = workflowStatus;
        StartDate = startDate;
        EndDate = endDate;
        PaymentFrequency = paymentFrequency;
        NextRunDate = GetNextRunDate(startDate, paymentFrequency);
    }

    public DateOnly CalculateNextRunDate()
    {
        return GetNextRunDate(NextRunDate, PaymentFrequency);
    }

    public DateOnly UpdateToNextRunDate()
    {
        NextRunDate = CalculateNextRunDate();
        return NextRunDate;
    }

    private DateOnly GetNextRunDate(DateOnly startDate, PaymentFrequency paymentFrequency)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        
        // If the start date is in the future, return it
        if (startDate > today)
        {
            return startDate;
        }

        DateOnly nextRun = startDate;

        switch (paymentFrequency)
        {
            case PaymentFrequency.Daily:
                nextRun = GetNextDailyRun(startDate, today);
                break;
                
            case PaymentFrequency.Weekly:
                nextRun = GetNextWeeklyRun(startDate, today);
                break;
                
            case PaymentFrequency.Monthly:
                nextRun = GetNextMonthlyRun(startDate, today);
                break;
                
            case PaymentFrequency.Yearly:
                nextRun = GetNextYearlyRun(startDate, today);
                break;
                
            default:
                throw new ArgumentException($"Unsupported payment frequency: {paymentFrequency}");
        }

        return nextRun;
    }

    private static DateOnly GetNextDailyRun(DateOnly startDate, DateOnly referenceDate)
    {
        var daysDiff = referenceDate.DayNumber - startDate.DayNumber;
        
        if (daysDiff < 0) return startDate;
        
        var nextRun = startDate.AddDays(daysDiff + 1);
        return nextRun;
    }

    private static DateOnly GetNextWeeklyRun(DateOnly startDate, DateOnly referenceDate)
    {
        var daysDiff = referenceDate.DayNumber - startDate.DayNumber;
        
        if (daysDiff < 0) return startDate;
        
        var weeksPassed = daysDiff / 7;
        var nextRun = startDate.AddDays((weeksPassed + 1) * 7);
        
        return nextRun;
    }

    private static DateOnly GetNextMonthlyRun(DateOnly startDate, DateOnly referenceDate)
    {
        var monthsDiff = ((referenceDate.Year - startDate.Year) * 12) + referenceDate.Month - startDate.Month;
        
        if (monthsDiff < 0) return startDate;
        
        var nextRun = startDate.AddMonths(monthsDiff + 1);
        
        // Handle cases where the day doesn't exist in the target month (e.g., Jan 31 -> Feb 31)
        if (nextRun.Day != startDate.Day)
        {
            var daysInMonth = DateTime.DaysInMonth(nextRun.Year, nextRun.Month);
            if (startDate.Day > daysInMonth)
            {
                nextRun = new DateOnly(nextRun.Year, nextRun.Month, daysInMonth);
            }
        }
        
        return nextRun;
    }

    private static DateOnly GetNextYearlyRun(DateOnly startDate, DateOnly referenceDate)
    {
        var yearsDiff = referenceDate.Year - startDate.Year;
        
        if (yearsDiff < 0) return startDate;
        
        var nextRun = startDate.AddYears(yearsDiff + 1);
        
        // Handle leap year scenario for Feb 29
        if (startDate.Month == 2 && startDate.Day == 29 && !DateTime.IsLeapYear(nextRun.Year))
        {
            nextRun = new DateOnly(nextRun.Year, 2, 28);
        }
        
        return nextRun;
    }

    public bool HasEnded()
    {
        return IsEnded || NextRunDate > EndDate;
    }

    public void InitializeNextRunDate()
    {
        NextRunDate = GetNextRunDate(StartDate, PaymentFrequency);
    }
}
