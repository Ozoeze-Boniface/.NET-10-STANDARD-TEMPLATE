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

    public RetryTransaction(long mandateId, bool isPosted, string? responseCode, string? responseDescription, DateTime? transactionDate, decimal amount, DateOnly nextRunDate, int retryCount)
    {
        MandateId = mandateId;
        IsPosted = isPosted;
        ResponseCode = responseCode;
        ResponseDescription = responseDescription;
        TransactionDate = transactionDate;
        Amount = amount;
        NextRunDate = nextRunDate;
        RetryCount = retryCount;
        DateCreated = DateOnly.FromDateTime(DateTime.UtcNow);
        TimeCreated = DateTimeOffset.UtcNow;
    }
}