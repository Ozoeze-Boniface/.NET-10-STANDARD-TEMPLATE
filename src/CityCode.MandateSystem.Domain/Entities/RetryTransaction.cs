namespace CityCode.MandateSystem.Domain.Entities;

public class RetryTransaction : BaseAuditableEntity
{
    public long RetryTransactionId { get; set; }
    public long MandateId { get; set; }
    public bool IsPosted { get; set; } = false;
    public string? ResponseCode { get; set; }
    public string? ResponseDescription { get; set; }
    public DateTime? TransactionDate { get; set; }
    public decimal Amount { get; set; }
    public DateOnly NextRunDate { get; set; }
    public int RetryCount { get; set; } = 0;
}