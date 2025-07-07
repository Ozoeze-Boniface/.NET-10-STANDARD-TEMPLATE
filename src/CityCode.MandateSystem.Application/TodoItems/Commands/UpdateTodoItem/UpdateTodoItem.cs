namespace CityCode.MandateSystem.Application.TodoItems.Commands.UpdateTodoItem;
using CityCode.MandateSystem.Application.Common.Interfaces;

public record UpdateTodoItemCommand : IRequest
{
    public int Id { get; init; }

    public string? Title { get; init; }

    public bool Done { get; init; }
}

public class UpdateTodoItemCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateTodoItemCommand>
{
    private readonly IApplicationDbContext _context = context;

    public async Task Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await this._context.TodoItems
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Title = request.Title;
        entity.Done = request.Done;

        await this._context.SaveChangesAsync(cancellationToken);
    }
}
