namespace KeyRails.BankingApi.Application.Queries;
using KeyRails.BankingApi.Application.Common.Interfaces;
using KeyRails.BankingApi.Application.Common.Mappings;
using KeyRails.BankingApi.Application.Common.Models;

public record GetApiRequestLogsWithPaginationQuery : IRequest<PaginatedList<ApiRequestLogDto>>
{
    public int ApiRequestLogId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetApiRequestLogsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetApiRequestLogsWithPaginationQuery, PaginatedList<ApiRequestLogDto>>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<PaginatedList<ApiRequestLogDto>> Handle(GetApiRequestLogsWithPaginationQuery request, CancellationToken cancellationToken) => await this._context.ApiRequestLogs
            .Where(x => x.ApiRequestLogId == request.ApiRequestLogId)
            .OrderBy(x => x.ApiRequestLogId)
            .ProjectTo<ApiRequestLogDto>(this._mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
}
