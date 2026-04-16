namespace KeyRails.BankingApi.Application.ApiRequestLogs.EventHandlers;
using Microsoft.Extensions.Logging;
using KeyRails.BankingApi.Domain.Events;

public class ApiRequestLogCompletedHandler(ILogger<ApiRequestLogCompletedHandler> logger) : INotificationHandler<ApiRequestLogCompletedEvent>
{
    private readonly ILogger<ApiRequestLogCompletedHandler> logger = logger;

    public Task Handle(ApiRequestLogCompletedEvent notification, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("KeyRails.BankingApi Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
