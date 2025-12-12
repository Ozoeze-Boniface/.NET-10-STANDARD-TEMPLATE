using CityCode.MandateSystem.Application.Common.Exceptions;
using CityCode.MandateSystem.Application.Dtos;
using CityCode.MandateSystem.Application.Services.UtilityServices.Interfaces;
using CityCode.MandateSystem.Domain.Enums;

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
        public decimal? NewInstallmentAmount { get; set; }
        public bool IsCharge { get; set; }
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
            if (request.LiquidationType == LiquidationType.FULL)
            {
                var sumOfTransaction = await _context.MandateTransactions.Where(t => t.MandateId == request.MandateId)
                    .SumAsync(s => s.Amount, cancellationToken: cancellationToken);
                amount = mandate.ProductTotalAmount - sumOfTransaction;
            }
            else
            {
                amount = request.Amount;
            }

            var mandateSchedule = await _context.MandateSchedules.FirstOrDefaultAsync(s => s.MandateId == request.MandateId, cancellationToken: cancellationToken) ?? throw new BadRequestException("Mandate does not have a valid schedule");

            var result = await _mandateService.DoFundsTransfer(mandate, amount: amount, isCharge: request.IsCharge);
            
            var transaction = new MandateTransaction(
                                mandateScheduleId: mandateSchedule.MandateScheduleId,
                                transactionReference: result.PaymentReference ?? string.Empty,
                                amount: result.Amount,
                                currency: "NGN",
                                transactionDate: DateOnly.FromDateTime(DateTime.Now),
                                transactionStatus: result.ResponseCode == "00"
                                    ? nameof(TransactionStatus.SUCCESSFUL)
                                    : nameof(TransactionStatus.FAILED), mandateSchedule.MandateId);
            transaction.UpdateFromResponse(result.ResponseCode, result.SessionID, result.ChannelCode,
                result.NameEnquiryRef, result.DestinationInstitutionCode, result.BeneficiaryAccountName,
                result.BeneficiaryAccountNumber, result.BeneficiaryKYCLevel,
                result.BeneficiaryBankVerificationNumber,
                result.OriginatorAccountName, result.OriginatorAccountNumber,
                result.OriginatorBankVerificationNumber, result.OriginatorKYCLevel,
                result.TransactionLocation, result.Narration, result.PaymentReference!);
            transaction.UpdateStatus(transaction.TransactionStatus, "SUCCESSFUL", result.TransactionId);

            await _context.MandateTransactions.AddAsync(transaction, cancellationToken);
            if (request.NewInstallmentAmount.HasValue)
            {
                var mandateRequest =
                    await _context.MandateRequests.FirstOrDefaultAsync(r =>
                        r.MandateReference == mandate.MandateReference, cancellationToken: cancellationToken);
                
                mandate.TransactionAmount = request.NewInstallmentAmount.Value;
                mandateRequest!.TransactionAmount = request.NewInstallmentAmount.Value;
            }
            await _context.SaveChangesAsync(cancellationToken);

            return Common.Models.View.Result<MandateTransactionResponse>.Success(DateTime.Now, result);
        }
    }
}