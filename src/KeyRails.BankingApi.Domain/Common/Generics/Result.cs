namespace KeyRails.BankingApi.Domain.Common;
public class Result<T>
{
    public T? Content { get; set; }
    public string? RequestId { get; set; } = "";
    public string? ErrorMessage { get; set; } = "";
    public bool HasError => this.ErrorMessage != "";
    public string? ResponseCode { get; set; }
    public string? ResponseDescription { get; set; }
    public DateTime? RequestTime { get; set; } = DateTime.UtcNow;
    public DateTime? ResponseTime { get; set; } //= DateTime.UtcNow;
    public TimeSpan? ActivityTime { get; set; } //;

    public bool? IsSuccess { get; set; } = true;
}

