using CityCode.MandateSystem.Domain.Enums;

namespace CityCode.MandateSystem.Application.Query;

public class GetMandateStatusCountQuery : IRequest<Common.Models.View.Result<MandateStatusResponse>>;

public class GetMandateStatusQueryHandler(IApplicationDbContext context) : IRequestHandler<GetMandateStatusCountQuery, Common.Models.View.Result<MandateStatusResponse>>
{
    public async Task<Common.Models.View.Result<MandateStatusResponse>> Handle(GetMandateStatusCountQuery request, CancellationToken cancellationToken)
    {
        var activeCount = await context.Mandates.CountAsync(m => m.WorkflowStatus == WorkflowStatus.MANDATE_APPROVED_BY_BANK, cancellationToken);
        var inactiveCount = await context.Mandates.CountAsync(m => m.WorkflowStatus == WorkflowStatus.BILLER_INITIATED, cancellationToken);
        var suspendedCount = await context.MandateSchedules.CountAsync(m => m.IsEnded, cancellationToken);

        var mandateStatus = new MandateStatusResponse(activeCount, inactiveCount, suspendedCount);
        return Common.Models.View.Result<MandateStatusResponse>.Success(DateTime.UtcNow, mandateStatus);
    }
}

public record MandateStatusResponse(int? activeCount, int? inactiveCount, int? suspendedCount);