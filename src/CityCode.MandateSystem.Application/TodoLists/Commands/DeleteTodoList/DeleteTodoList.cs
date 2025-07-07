namespace CityCode.MandateSystem.Application.TodoLists.Commands.DeleteTodoList;
using CityCode.MandateSystem.Application.Common.Interfaces;

public record DeleteTodoListCommand(int Id) : IRequest;

public class DeleteTodoListCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteTodoListCommand>
{
    private readonly IApplicationDbContext _context = context;

    public async Task Handle(DeleteTodoListCommand request, CancellationToken cancellationToken)
    {
        var entity = await this._context.TodoLists
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        this._context.TodoLists.Remove(entity);

        await this._context.SaveChangesAsync(cancellationToken);
    }
}
