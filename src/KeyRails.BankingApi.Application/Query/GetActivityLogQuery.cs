using KeyRails.BankingApi.Application.Common.Mappings;
using KeyRails.BankingApi.Application.Common.Models;
using KeyRails.BankingApi.Application.Extentions;

namespace KeyRails.BankingApi.Application.Query
{
    public class GetActivityLogQuery : IRequest<Common.Models.View.Result<PaginatedList<Activity>>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public long? ActorId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class GetActivityLogQueryHandler(IApplicationDbContext context) : IRequestHandler<GetActivityLogQuery, Common.Models.View.Result<PaginatedList<Activity>>>
    {
        private readonly IApplicationDbContext _context = context;

        public async Task<Common.Models.View.Result<PaginatedList<Activity>>> Handle(GetActivityLogQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Activities.AsQueryable().ApplyActivityFilter(request);

            var result = await query.PaginatedListAsync(request.PageNumber ?? 1, request.PageSize ?? 50);

            return Common.Models.View.Result<PaginatedList<Activity>>.Success(DateTime.Now, result);
        }
    }
}