namespace KeyRails.BankingApi.Application.TodoItems.Queries.GetTodoItemsWithPagination;
using KeyRails.BankingApi.Application.Common.Interfaces;
using KeyRails.BankingApi.Application.Common.Mappings;
using KeyRails.BankingApi.Application.Common.Models;

public record GetTodoItemsWithPaginationQuery : IRequest<PaginatedList<TodoItemBriefDto>>
{
    public int ListId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetTodoItemsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetTodoItemsWithPaginationQuery, PaginatedList<TodoItemBriefDto>>
{
    private readonly IApplicationDbContext context = context;
    private readonly IMapper mapper = mapper;

    public async Task<PaginatedList<TodoItemBriefDto>> Handle(GetTodoItemsWithPaginationQuery request, CancellationToken cancellationToken) => await this.context.TodoItems
            .Where(x => x.ListId == request.ListId)
            .OrderBy(x => x.Title)
            .ProjectTo<TodoItemBriefDto>(this.mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
}
