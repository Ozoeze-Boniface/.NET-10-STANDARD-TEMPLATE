namespace KeyRails.BankingApi.Application.TodoLists.Commands.CreateTodoList;
using KeyRails.BankingApi.Application.Common.Interfaces;
using KeyRails.BankingApi.Domain.Entities;

public record CreateTodoListCommand : IRequest<int>
{
    public string? Title { get; init; }
}

public class CreateTodoListCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateTodoListCommand, int>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<int> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
    {
        var entity = new TodoList
        {
            Title = request.Title
        };

        this._context.TodoLists.Add(entity);

        await this._context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
