using CityCode.MandateSystem.Application.Common.Exceptions;
using CityCode.MandateSystem.Domain.Enums;

namespace CityCode.MandateSystem.Application.Commands
{
    public class RejectMandateRequestCommand : IRequest<Common.Models.View.Result<string>>
    {
        public long MandateRequestId { get; set; }
        public string? Reason { get; set; }
    }

    public class RejectMandateRequestCommandHandler(IApplicationDbContext context) : IRequestHandler<RejectMandateRequestCommand, Common.Models.View.Result<string>>
    {
        public async Task<Common.Models.View.Result<string>> Handle(RejectMandateRequestCommand request, CancellationToken cancellationToken)
        {
            var mandateRequest = await context.MandateRequests.FirstOrDefaultAsync(
                mr => mr.MandateRequestId == request.MandateRequestId, cancellationToken: cancellationToken) ?? throw new BadRequestException("Request Not found");
            mandateRequest.UpdateMandateRequestStatus(MandateRequestStatus.REJECTED);
            // mandateRequest.Narration = request.Reason??"Rejected Mandate";

            await context.SaveChangesAsync(cancellationToken);

            return Common.Models.View.Result<string>.Success(DateTime.UtcNow, "Rejection applied");
        }
    }
}