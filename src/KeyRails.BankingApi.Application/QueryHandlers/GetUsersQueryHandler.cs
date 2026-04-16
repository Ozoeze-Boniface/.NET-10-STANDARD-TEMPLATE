using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeyRails.BankingApi.Application.Common.Mappings;
using KeyRails.BankingApi.Application.Common.Models;
using KeyRails.BankingApi.Application.Common.Models.View;
using KeyRails.BankingApi.Application.Extentions;
using KeyRails.BankingApi.Application.Query;

namespace KeyRails.BankingApi.Application.QueryHandlers
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