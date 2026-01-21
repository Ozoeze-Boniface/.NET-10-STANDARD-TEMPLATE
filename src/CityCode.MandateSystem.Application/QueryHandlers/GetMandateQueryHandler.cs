using CityCode.MandateSystem.Application.Common.Mappings;
using CityCode.MandateSystem.Application.Common.Models;
using CityCode.MandateSystem.Application.Extentions;
using CityCode.MandateSystem.Application.Query;

namespace CityCode.MandateSystem.Application.QueryHandlers
{
    public class GetMandateQueryHandler(IApplicationDbContext context) : IRequestHandler<GetMandateQuery, Common.Models.View.Result<PaginatedList<Mandate>>>
    {
        public async Task<Common.Models.View.Result<PaginatedList<Mandate>>> Handle(GetMandateQuery request, CancellationToken cancellationToken)
        {
            var query = context.Mandates.Include(d => d.Documents).AsQueryable().ApplyFilters(request);

            var result = await query.PaginatedListAsync(request.PageNumber, request.PageSize);

            return Common.Models.View.Result<PaginatedList<Mandate>>.Success(DateTime.Now, result);
        }
    }
}