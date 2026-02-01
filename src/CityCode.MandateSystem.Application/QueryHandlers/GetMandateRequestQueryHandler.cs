using CityCode.MandateSystem.Application.Common.Mappings;
using CityCode.MandateSystem.Application.Common.Models;
using CityCode.MandateSystem.Application.Extentions;
using CityCode.MandateSystem.Application.Query;

namespace CityCode.MandateSystem.Application.QueryHandlers
{
    public class GetMandateRequestQueryHandler(IApplicationDbContext context) : IRequestHandler<GetMandateRequestQuery, Common.Models.View.Result<PaginatedList<MandateRequest>>>
    {
        public async Task<Common.Models.View.Result<PaginatedList<MandateRequest>>> Handle(GetMandateRequestQuery request, CancellationToken cancellationToken)
        {
            var query = context.MandateRequests.AsNoTracking().AsQueryable().ApplyFilters(request);

            var result = await query.PaginatedListAsync(request.PageNumber, request.PageSize);

            return Common.Models.View.Result<PaginatedList<MandateRequest>>.Success(DateTime.Now, result);
        }
    }
}