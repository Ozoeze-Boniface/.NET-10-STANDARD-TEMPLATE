namespace CityCode.MandateSystem.Application.TodoItems.EventHandlers;
using Microsoft.Extensions.Logging;
using CityCode.MandateSystem.Domain.Events;

public class TodoItemCompletedHandler(ILogger<TodoItemCompletedHandler> logger) : INotificationHandler<TodoItemCompletedEvent>
{
    private readonly ILogger<TodoItemCompletedHandler> logger = logger;

    public Task Handle(TodoItemCompletedEvent notification, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("CityCode.MandateSystem Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
