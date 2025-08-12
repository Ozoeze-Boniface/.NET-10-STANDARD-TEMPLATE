using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityCode.MandateSystem.Application.Common.Models.View;

namespace CityCode.MandateSystem.Application.Query
{
    public class GetBankQuery : IRequest<Common.Models.View.Result<List<Bank>>>
    {
    }

    public class GetBankQueryHandler(IApplicationDbContext context) : IRequestHandler<GetBankQuery, Common.Models.View.Result<List<Bank>>>
    {
        private readonly IApplicationDbContext _context = context;

        public async Task<Common.Models.View.Result<List<Bank>>> Handle(GetBankQuery request, CancellationToken cancellationToken)
        {
            var banks = await _context.Banks.AsNoTracking().ToListAsync();

            return Common.Models.View.Result<List<Bank>>.Success(DateTime.Now, banks);
        }
    }
}