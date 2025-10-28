using CityCode.MandateSystem.Application.Commands;
using CityCode.MandateSystem.Application.Common.Exceptions;
using CityCode.MandateSystem.Application.Dtos;
using CityCode.MandateSystem.Application.Services.UtilityServices.Interfaces;
using CityCode.MandateSystem.Domain.Enums;

namespace CityCode.MandateSystem.Application.CommandHandlers
{
    public class ApproveMandateCommandHandler : IRequestHandler<ApproveMandateCommand, Common.Models.View.Result<object>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMandateService _mandateService;
        private readonly ILogger<ApproveMandateCommandHandler> _logger;
        private readonly IEmailService _emailService;

        public ApproveMandateCommandHandler(IApplicationDbContext context, IMapper mapper, IMandateService mandateService, ILogger<ApproveMandateCommandHandler> logger, IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _mandateService = mandateService;
            _logger = logger;
            _emailService = emailService;
        }

        public async Task<Common.Models.View.Result<object>> Handle(ApproveMandateCommand request, CancellationToken cancellationToken)
        {
            var mandateRequest = await _context.MandateRequests.FindAsync(request.MandateId);
            if (mandateRequest == null)
            {
                throw new BadRequestException("Mandate request not found");
            }
            var mandateExists = await _context.Mandates.AnyAsync(m => m.MandateReference == mandateRequest.MandateReference);
            if (mandateExists)
            {
                throw new BadRequestException("Mandate already exixts");
            }
            var mandate = _mapper.Map<Mandate>(mandateRequest);
            mandate.UpdateMandateStatus(MandateStatus.INACTIVE);
            mandate.UpdateApproverDetails(request.ApproverId, request.ApprovedBy);

            // 1. CALL NIBBS TO CREATE MANDATE AND ALSO CALL NIP UPDATE TO UPDATE STATUS
            var mandateCreationResponse = await _mandateService.CreateMandateAsync(mandate);
            mandate.UpdateNibbsMandateCode(mandateCreationResponse.Data?.MandateCode!);
            // 2. UPDATE MANDATE REQUEST STATUS TO APPROVED
            mandateRequest.UpdateMandateRequestStatus(MandateRequestStatus.APPROVED);
            // 3. UPDATE MANDATE STATUS TO ACTIVE IF NIP UPDATE IS SUCCESSFUL
            var mandateActivationResponse = await _mandateService.ActivateMandate(mandate);
            mandate.UpdateMandateStatus(MandateStatus.ACTIVE);
            // 4. UPDATE MANDATE PROGRESS STATUS TO NIBBS_APPROVED IF SUCCESSFUL, ELSE NIBBS_REJECTED OR NIBBS_ERROR
            mandate.SetProgressStatus(ProgressStatus.NIBBS_APPROVED);
            // 5. CALL GET STATUS AND UPDATE MANDATE WORKFLOW STATUS
            var mandateStatusResponse = await _mandateService.GetMandateStatus(mandate.NibbsMandateCode!);
            _logger.LogInformation("Mandate Status Response: {Response}", JsonConvert.SerializeObject(mandateStatusResponse));
            mandate.UpdateWorkflow(WorkflowStatus.BILLER_INITIATED);

            //INSERT INTO SCHEDULE TABLE
            var schedule = new MandateSchedule(mandate.MandateId, mandate.MandateReference, mandate.NibbsMandateCode!, mandate.WorkflowStatus!.Value, mandate.StartDate, mandate.EndDate, mandate.PaymentFrequency);

            schedule.Mandate = mandate;
            await _context.Mandates.AddAsync(mandate, cancellationToken);
            await _context.MandateSchedules.AddAsync(schedule, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            _ = Task.Run(async () => await _emailService.SendEmail(mandate.PayerEmail,
                new MailContent()
                {
                    Body = mandateCreationResponse.Data?.Description ?? string.Empty,
                    Header = "Mandate Successfully Created", Subject = "Mandate Creation Successful"
                }), cancellationToken);
            return Common.Models.View.Result<object>.Success(DateTime.UtcNow,
            new
            {
                MandateCreationResponse = mandateCreationResponse,
                MandateActivationResponse = mandateActivationResponse,
                MandateStatusResponse = mandateStatusResponse
            },
            "Mandate approved successfully");
        }
    }
}