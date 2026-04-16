namespace KeyRails.BankingApi.Domain.Entities;
public class ApiRequestLog : BaseEntity
{
    public int ApiRequestLogId { get; set; }
    public string? ApiUserId { get; set; }
    public string? ClientUniqueRequestId { get; set; }
    public string? EndPointUrl { get; set; }
    public string? ApiRequestJson { get; set; }
    public DateTime ApiRequestDate { get; set; }
    public DateTime ApiRequestTime { get; set; }
    public string? ApiResponseJson { get; set; }
    public DateTime? ApiResponseDate { get; set; }
    public DateTime? ApiResponseTime { get; set; }
    public string? ApiCallStatus { get; set; }
    public ulong RowVersion { get; set; }

}
