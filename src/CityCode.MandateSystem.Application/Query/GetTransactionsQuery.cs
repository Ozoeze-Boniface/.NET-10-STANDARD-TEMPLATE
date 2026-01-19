using CityCode.MandateSystem.Application.Common.Mappings;
using CityCode.MandateSystem.Application.Common.Models;
using CityCode.MandateSystem.Application.Extentions;

namespace CityCode.MandateSystem.Application.Query
{
    public class GetTransactionsQuery : IRequest<Common.Models.View.Result<PaginatedList<MandateTransaction>>>
    {
        public long? MandateId { get; set; }
        public string? TransactionStatus { get; set; }
        public string? CustomerName { get; set; } = default!;
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
    }

    public class GetTransactionsQueryHandler(IApplicationDbContext context)
        : IRequestHandler<GetTransactionsQuery, Common.Models.View.Result<PaginatedList<MandateTransaction>>>
    {
        public async Task<Common.Models.View.Result<PaginatedList<MandateTransaction>>> Handle(GetTransactionsQuery request,
            CancellationToken cancellationToken)
        {
            request.PageSize ??= 200;
            request.PageNumber ??= 1;
            var query = context.MandateTransactions.AsQueryable().ApplyTransactionFilter(request);

            var result = await query.PaginatedListAsync(request.PageNumber.Value, request.PageSize.Value);

            return Common.Models.View.Result<PaginatedList<MandateTransaction>>.Success(DateTime.Now, result);

        }
    }
}