namespace CityCode.MandateSystem.Application.TodoItems.Commands.UpdateTodoItemDetail;
using CityCode.MandateSystem.Application.Common.Interfaces;
using CityCode.MandateSystem.Domain.Enums;
public record UpdateTodoItemDetailCommand : IRequest
{
    public int Id { get; init; }

    public int ListId { get; init; }

    public PriorityLevel Priority { get; init; }

    public string? Note { get; init; }
}

public class UpdateTodoItemDetailCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateTodoItemDetailCommand>
{
    private readonly IApplicationDbContext _context = context;

    public async Task Handle(UpdateTodoItemDetailCommand request, CancellationToken cancellationToken)
    {
        var entity = await this._context.TodoItems
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.ListId = request.ListId;
        entity.Priority = request.Priority;
        entity.Note = request.Note;

        await this._context.SaveChangesAsync(cancellationToken);
    }
}
