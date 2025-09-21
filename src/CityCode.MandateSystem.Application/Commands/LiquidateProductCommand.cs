using CityCode.MandateSystem.Application.Common.Exceptions;
using CityCode.MandateSystem.Application.Dtos;
using CityCode.MandateSystem.Application.Services.UtilityServices.Interfaces;

namespace CityCode.MandateSystem.Application.Commands
{
    public enum LiquidationType
    {
        PARTIAL,
        FULL
    }
    public class LiquidateProductCommand : IRequest<Common.Models.View.Result<MandateTransactionResponse>>
    {
        public long MandateId { get; set; }
        public decimal Amount { get; set; }
        public LiquidationType? LiquidationType { get; set; }
    }

    public class LiquidateProductCommandHandler(IApplicationDbContext context, IMandateService mandateService) : IRequestHandler<LiquidateProductCommand, Common.Models.View.Result<MandateTransactionResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMandateService _mandateService = mandateService;

        public async Task<Common.Models.View.Result<MandateTransactionResponse>> Handle(LiquidateProductCommand request, CancellationToken cancellationToken)
        {
            var mandate = await _context.Mandates.FindAsync(request.MandateId, cancellationToken);

            if (mandate is null)
            {
                throw new BadRequestException("Mandate does not exist");

            }

            decimal amount;
            if(request.LiquidationType == LiquidationType.FULL)
            {
                var sumOfTransaction = await _context.MandateTransactions.Where(t => t.MandateId == request.MandateId)
                    .SumAsync(s => s.Amount, cancellationToken: cancellationToken);
                amount = mandate.ProductTotalAmount - sumOfTransaction;
            }
            else
            {
                amount = request.Amount;
            }
            var result = await _mandateService.DoFundsTransfer(mandate, amount: amount);

            return Common.Models.View.Result<MandateTransactionResponse>.Success(DateTime.Now, result);
        }
    }
}