namespace CityCode.MandateSystem.Application.TodoLists.Commands.PurgeTodoLists;
using CityCode.MandateSystem.Application.Common.Interfaces;
using CityCode.MandateSystem.Application.Common.Security;
using CityCode.MandateSystem.Domain.Constants;

[Authorize(Roles = Roles.Administrator)]
[Authorize(Policy = Policies.CanPurge)]
public record PurgeTodoListsCommand : IRequest;

public class PurgeTodoListsCommandHandler(IApplicationDbContext context) : IRequestHandler<PurgeTodoListsCommand>
{
    private readonly IApplicationDbContext _context = context;

    public async Task Handle(PurgeTodoListsCommand request, CancellationToken cancellationToken)
    {
        this._context.TodoLists.RemoveRange(this._context.TodoLists);

        await this._context.SaveChangesAsync(cancellationToken);
    }
}
