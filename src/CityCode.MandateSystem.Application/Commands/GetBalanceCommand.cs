using CityCode.MandateSystem.Application.Common.Exceptions;
using CityCode.MandateSystem.Domain.Enums;

namespace CityCode.MandateSystem.Application.Commands
{
    public class GetBalanceCommand : IRequest<Common.Models.View.Result<Balances>>
    {
        public long MandateId { get; set; }
    }

    public record Balances(decimal RemainingPrincipal, decimal AmountPaid);

    public class GetBalanceCommandHandler(IApplicationDbContext context) : IRequestHandler<GetBalanceCommand, Common.Models.View.Result<Balances>>
    {
        public async Task<Common.Models.View.Result<Balances>> Handle(GetBalanceCommand request, CancellationToken cancellationToken)
        {
            var mandate = await context.Mandates.AsNoTracking().FirstOrDefaultAsync(m => m.MandateId == request.MandateId, cancellationToken: cancellationToken);

            if (mandate is null)
            {
                throw new BadRequestException("Mandate not found");
            }

            var mandateTransactions = await context.MandateTransactions.AsNoTracking()
                .Where(mt => mt.MandateId == request.MandateId && mt.TransactionStatus == nameof(TransactionStatus.SUCCESSFUL)).ToListAsync(cancellationToken: cancellationToken);

            var totalTransactionsAmount = mandateTransactions.Sum(a => a.Amount);
            var remainingPrincipal = mandate.ProductTotalAmount - totalTransactionsAmount;

            return Common.Models.View.Result<Balances>.Success(DateTime.UtcNow,
                new Balances(remainingPrincipal, totalTransactionsAmount));

        }
    }
}