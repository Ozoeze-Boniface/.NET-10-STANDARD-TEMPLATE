using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityCode.MandateSystem.Application.Common.Mappings;
using CityCode.MandateSystem.Application.Common.Models;
using CityCode.MandateSystem.Application.Common.Models.View;
using CityCode.MandateSystem.Application.Extentions;
using CityCode.MandateSystem.Application.Query;

namespace CityCode.MandateSystem.Application.QueryHandlers
{
    public class GetMandateQueryHandler(IApplicationDbContext context) : IRequestHandler<GetMandateQuery, Common.Models.View.Result<PaginatedList<Mandate>>>
    {
        private readonly IApplicationDbContext _context = context;

        public async Task<Common.Models.View.Result<PaginatedList<Mandate>>> Handle(GetMandateQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Mandates.AsQueryable().ApplyFilters(request);

            var result = await query.PaginatedListAsync(request.PageNumber, request.PageSize);

            return Common.Models.View.Result<PaginatedList<Mandate>>.Success(DateTime.Now, result);

            throw new NotImplementedException();
        }
    }
}