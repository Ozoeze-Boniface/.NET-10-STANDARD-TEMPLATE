namespace KeyRails.BankingApi.Application.TodoItems.Commands.CreateTodoItem;
using KeyRails.BankingApi.Application.Common.Interfaces;
using KeyRails.BankingApi.Domain.Entities;
using KeyRails.BankingApi.Domain.Events;

public record CreateTodoItemCommand : IRequest<int>
{
    public int ListId { get; init; }

    public string? Title { get; init; }
}

public class CreateTodoItemCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateTodoItemCommand, int>
{
    private readonly IApplicationDbContext context = context;

    public async Task<int> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = new TodoItem
        {
            ListId = request.ListId,
            Title = request.Title,
            Done = false
        };

        entity.AddDomainEvent(new TodoItemCreatedEvent(entity));

        this.context.TodoItems.Add(entity);

        await this.context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
