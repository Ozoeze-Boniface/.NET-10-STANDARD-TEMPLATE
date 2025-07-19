using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityCode.MandateSystem.Application.Commands;
using CityCode.MandateSystem.Domain.Enums;

namespace CityCode.MandateSystem.Application.CommandHandlers
{
    public class ApproveMandateCommandHandler : IRequestHandler<ApproveMandateCommand, Common.Models.View.Result<Mandate>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ApproveMandateCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Common.Models.View.Result<Mandate>> Handle(ApproveMandateCommand request, CancellationToken cancellationToken)
        {
            var mandateRequest = await _context.MandateRequests.FindAsync(request.MandateId);
            if (mandateRequest == null)
            {
                return Common.Models.View.Result<Mandate>.Failure("Mandate not found");
            }

            var mandate = _mapper.Map<Mandate>(mandateRequest);
            mandate.UpdateMandateStatus(MandateStatus.INACTIVE);
            mandate.UpdateApproverDetails(request.ApproverId, request.ApprovedBy);

            // 1. CALL NIBBS TO CREATE MANDATE AND ALSO CALL NIP UPDATE TO UPDATE STATUS
            // 2. UPDATE MANDATE REQUEST STATUS TO APPROVED
            //mandateRequest.UpdateMandateRequestStatus(MandateRequestStatus.APPROVED);
            // 3. UPDATE MANDATE STATUS TO ACTIVE IF NIP UPDATE IS SUCCESSFUL
            //mandate.UpdateMandateStatus(MandateStatus.ACTIVE);
            // 4. UPDATE MANDATE PROGRESS STATUS TO NIBBS_APPROVED IF SUCCESSFUL, ELSE NIBBS_REJECTED OR NIBBS_ERROR
            // mandate.SetProgressStatus(ProgressStatus.NIBBS_APPROVED);
            // 5. UPDATE MANDATE NIBBS CODE IF SUCCESSFUL
            //mandate.UpdateNibbsMandateCode("HFHHJDJDHDJDJDJDJDJDJDJDJDJDJD"); // This should be replaced with actual NIBBS mandate code after API call

            await _context.Mandates.AddAsync(mandate);

            await _context.SaveChangesAsync(cancellationToken);
            return Common.Models.View.Result<Mandate>.Success(DateTime.UtcNow, mandate);
        }
    }
}