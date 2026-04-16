namespace KeyRails.BankingApi.Application.ApiRequestLogs.EventHandlers;
public class ApiRequestLogCreatedHandler(ILogger<ApiRequestLogCreatedHandler> logger) : INotificationHandler<ApiRequestLogCreatedEvent>
{
    private readonly ILogger<ApiRequestLogCreatedHandler> logger = logger;

    public Task Handle(ApiRequestLogCreatedEvent notification, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("KeyRails.BankingApi Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
