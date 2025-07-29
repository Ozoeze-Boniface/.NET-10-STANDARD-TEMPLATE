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
    public class GetUsersQueryHandler(IApplicationDbContext context) : IRequestHandler<GetUsersQuery, Common.Models.View.Result<PaginatedList<User>>>
    {
        private readonly IApplicationDbContext _context = context;

        public async Task<Common.Models.View.Result<PaginatedList<User>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var userQuery = _context.AppUsers.Include(p => p.Permission).AsQueryable().ApplyUserFilter(request);

            var result = await userQuery.PaginatedListAsync(request.PageNumber ?? 1, request.PageSize ?? 50);

            return Common.Models.View.Result<PaginatedList<User>>.Success(DateTime.UtcNow, result);
        }
    }
}