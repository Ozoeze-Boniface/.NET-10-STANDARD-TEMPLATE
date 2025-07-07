namespace CityCode.MandateSystem.Application.ApiRequestLogs.EventHandlers;
public class ApiRequestLogCreatedHandler(ILogger<ApiRequestLogCreatedHandler> logger) : INotificationHandler<ApiRequestLogCreatedEvent>
{
    private readonly ILogger<ApiRequestLogCreatedHandler> logger = logger;

    public Task Handle(ApiRequestLogCreatedEvent notification, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("CityCode.MandateSystem Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
