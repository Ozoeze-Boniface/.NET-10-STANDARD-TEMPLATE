namespace CityCode.MandateSystem.Application.TodoItems.EventHandlers;
using Microsoft.Extensions.Logging;
using CityCode.MandateSystem.Domain.Events;

public class TodoItemCreatedHandler(ILogger<TodoItemCreatedHandler> logger) : INotificationHandler<TodoItemCreatedEvent>
{
    private readonly ILogger<TodoItemCreatedHandler> logger = logger;

    public Task Handle(TodoItemCreatedEvent notification, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("CityCode.MandateSystem Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
