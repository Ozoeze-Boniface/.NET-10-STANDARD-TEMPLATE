using CityCode.MandateSystem.Application.Common.Exceptions;
using CityCode.MandateSystem.Domain.Enums;

namespace CityCode.MandateSystem.Application.Commands
{
    public class DeactivateMandateCommand : IRequest<Common.Models.View.Result<string>>
    {
        public MandateStatus MandateStatus { get; set; }
        public long MandateId { get; set; }
    }

    public class DeactivateMandateCommandHandler(IApplicationDbContext context) : IRequestHandler<DeactivateMandateCommand, Common.Models.View.Result<string>>
    {
        public async Task<Common.Models.View.Result<string>> Handle(DeactivateMandateCommand request, CancellationToken cancellationToken)
        {
            var mandate = await context.Mandates.FindAsync(new object?[] { request.MandateId, cancellationToken }, cancellationToken: cancellationToken);
            if(mandate is null)
            {
                throw new BadRequestException("Mandate does not exist");
            }

            mandate.UpdateMandateStatus(request.MandateStatus);
            var schedule = await context.MandateSchedules.FirstOrDefaultAsync(m => m.MandateId == request.MandateId);
            if(schedule is not null)
            {
                schedule.IsEnded = true;
            }

            await context.SaveChangesAsync(cancellationToken);

            return Common.Models.View.Result<string>.Success(DateTime.Now, "Mandate deactivated");
        }
    }
}