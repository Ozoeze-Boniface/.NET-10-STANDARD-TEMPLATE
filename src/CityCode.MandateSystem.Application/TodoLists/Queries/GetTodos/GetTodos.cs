namespace CityCode.MandateSystem.Application.TodoLists.Queries.GetTodos;
using CityCode.MandateSystem.Application.Common.Interfaces;
using CityCode.MandateSystem.Application.Common.Models;
using CityCode.MandateSystem.Application.Common.Security;
using CityCode.MandateSystem.Domain.Enums;

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
