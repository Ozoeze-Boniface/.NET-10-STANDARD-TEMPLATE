namespace CityCode.MandateSystem.Application.TodoLists.Commands.UpdateTodoList;
//using CityCode.MandateSystem.Application.Common.Interfaces;

public record UpdateTodoListCommand : IRequest
{
    public int Id { get; init; }

    public string? Title { get; init; }
}

public class UpdateTodoListCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateTodoListCommand>
{
    private readonly IApplicationDbContext _context = context;

    public async Task Handle(UpdateTodoListCommand request, CancellationToken cancellationToken)
    {
        var entity = await this._context.TodoLists
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Title = request.Title;

        await this._context.SaveChangesAsync(cancellationToken);

    }
}
