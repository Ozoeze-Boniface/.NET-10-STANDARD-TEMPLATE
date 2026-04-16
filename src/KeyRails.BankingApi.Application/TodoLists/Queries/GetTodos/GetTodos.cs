namespace KeyRails.BankingApi.Application.TodoLists.Queries.GetTodos;
using KeyRails.BankingApi.Application.Common.Interfaces;
using KeyRails.BankingApi.Application.Common.Models;
using KeyRails.BankingApi.Application.Common.Security;
using KeyRails.BankingApi.Domain.Enums;

[Authorize]
public record GetTodosQuery : IRequest<TodosVm>;

public class GetTodosQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetTodosQuery, TodosVm>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<TodosVm> Handle(GetTodosQuery request, CancellationToken cancellationToken) => new TodosVm
    {
        PriorityLevels = Enum.GetValues(typeof(PriorityLevel))
                .Cast<PriorityLevel>()
                .Select(p => new LookupDto { Id = (int)p, Title = p.ToString() })
                .ToList(),

        Lists = await this._context.TodoLists
                .AsNoTracking()
                .ProjectTo<TodoListDto>(this._mapper.ConfigurationProvider)
                .OrderBy(t => t.Title)
                .ToListAsync(cancellationToken)
    };
}
