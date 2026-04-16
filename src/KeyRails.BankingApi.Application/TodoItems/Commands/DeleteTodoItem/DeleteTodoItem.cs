namespace KeyRails.BankingApi.Application.TodoItems.Commands.DeleteTodoItem;
using KeyRails.BankingApi.Application.Common.Interfaces;
using KeyRails.BankingApi.Domain.Events;

public record DeleteTodoItemCommand(int Id) : IRequest;

public class DeleteTodoItemCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteTodoItemCommand>
{
    private readonly IApplicationDbContext context = context;

    public async Task Handle(DeleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await this.context.TodoItems
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        this.context.TodoItems.Remove(entity);

        entity.AddDomainEvent(new TodoItemDeletedEvent(entity));

        await this.context.SaveChangesAsync(cancellationToken);
    }

}
