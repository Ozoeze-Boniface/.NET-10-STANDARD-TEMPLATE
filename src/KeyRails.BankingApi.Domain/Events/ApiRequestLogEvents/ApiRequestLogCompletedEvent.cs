namespace KeyRails.BankingApi.Domain.Events;

public class ApiRequestLogCompletedEvent(ApiRequestLog item) : BaseEvent
{
    public ApiRequestLog Item { get; } = item;
}
