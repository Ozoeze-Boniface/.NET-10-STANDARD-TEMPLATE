namespace KeyRails.BankingApi.Application.TodoLists.Commands.PurgeTodoLists;
using KeyRails.BankingApi.Application.Common.Interfaces;
using KeyRails.BankingApi.Application.Common.Security;
using KeyRails.BankingApi.Domain.Constants;

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
