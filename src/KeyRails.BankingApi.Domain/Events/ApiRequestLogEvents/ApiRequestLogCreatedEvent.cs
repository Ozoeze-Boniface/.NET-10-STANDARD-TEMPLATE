namespace KeyRails.BankingApi.Domain.Events;

public class ApiRequestLogCreatedEvent(ApiRequestLog item) : BaseEvent
{
    public ApiRequestLog Item { get; } = item;
}
