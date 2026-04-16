namespace KeyRails.BankingApi.Application.TodoItems.EventHandlers;
using Microsoft.Extensions.Logging;
using KeyRails.BankingApi.Domain.Events;

public class TodoItemCreatedHandler(ILogger<TodoItemCreatedHandler> logger) : INotificationHandler<TodoItemCreatedEvent>
{
    private readonly ILogger<TodoItemCreatedHandler> logger = logger;

    public Task Handle(TodoItemCreatedEvent notification, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("KeyRails.BankingApi Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
