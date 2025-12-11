using CityCode.MandateSystem.Application.Commands;
using CityCode.MandateSystem.Application.Settings;
using CityCode.MandateSystem.Domain.Enums;
using CityCode.MandateSystem.Domain.Events.ActivityLog;
using Microsoft.Extensions.Options;

namespace CityCode.MandateSystem.Application.Commands
{
public class EditMandateCommand : IRequest<Common.Models.View.Result<MandateRequest>>
    {
        public long MandateRequestId { get; set; }
        public string InitiatedBy { get; set; } = string.Empty;
        public long InitiatedById { get; set; }
        public string SubscriberCode { get; set; } = string.Empty;
        public decimal ProductTotalAmount { get; set; }
        public decimal TransactionAmount { get; set; }
        public string BankCode { get; set; } = string.Empty;
        public string PayerName { get; set; } = string.Empty;
        public string PayerAddress { get; set; } = string.Empty;
        public string PayerEmail { get; set; } = string.Empty;
        public string PayerPhoneNumber { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public string PayerAccountNumber { get; set; } = string.Empty;
        public string PayerBvn { get; set; } = string.Empty;
        public string BanksAccountNumber { get; set; } = string.Empty;
        public string BanksAccountName { get; set; } = string.Empty;
        public string BanksBvn { get; set; } = string.Empty;
        public string DestinationInstitutionCode { get; set; } = string.Empty;
        public string SourceInstitutionCode { get; set; } = string.Empty;
        public string Narration { get; set; } = string.Empty;
        public int MandateType { get; set; }
        public MandateRequestStatus MandateRequestStatus { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public PaymentFrequency PaymentFrequency { get; set; }
        public string Location { get; set; } = string.Empty;
        public bool TakeCharge { get; set; }
    }

    public class EditMandateCommandValidator : AbstractValidator<EditMandateCommand>
    {
        public EditMandateCommandValidator()
        {
            RuleFor(x => x.MandateRequestId)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");

            RuleFor(x => x.InitiatedBy)
                .NotEmpty().WithMessage("InitiatedBy is required.")
                .MaximumLength(100);

            RuleFor(x => x.InitiatedById)
                .GreaterThan(0).WithMessage("InitiatedById must be greater than 0.");

            RuleFor(x => x.SubscriberCode)
                .NotEmpty().WithMessage("SubscriberCode is required.")
                .MaximumLength(50);

            RuleFor(x => x.ProductTotalAmount)
                .GreaterThan(0).WithMessage("ProductTotalAmount must be greater than 0.");

            RuleFor(x => x.TransactionAmount)
                .GreaterThan(0).WithMessage("TransactionAmount must be greater than 0.");

            RuleFor(x => x.BankCode)
                .NotEmpty().WithMessage("BankCode is required.")
                .MaximumLength(20);

            RuleFor(x => x.PayerName)
                .NotEmpty().WithMessage("PayerName is required.")
                .MaximumLength(100);

            RuleFor(x => x.PayerAddress)
                .NotEmpty().WithMessage("PayerAddress is required.")
                .MaximumLength(200);

            RuleFor(x => x.PayerEmail)
                .NotEmpty().WithMessage("PayerEmail is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(100);

            RuleFor(x => x.PayerPhoneNumber)
                .NotEmpty().WithMessage("PayerPhoneNumber is required.")
                .MaximumLength(11);

            RuleFor(x => x.AccountName)
                .NotEmpty().WithMessage("AccountName is required.")
                .MaximumLength(100);

            RuleFor(x => x.PayerAccountNumber)
                .NotEmpty().WithMessage("PayerAccountNumber is required.")
                .Length(10).WithMessage("PayerAccountNumber must be exactly 10 digits.");

            RuleFor(x => x.PayerBvn)
                .NotEmpty().WithMessage("PayerBvn is required.")
                .Length(11).WithMessage("PayerBvn must be exactly 11 digits.");

            RuleFor(x => x.BanksAccountNumber)
                .NotEmpty().WithMessage("BanksAccountNumber is required.")
                .Length(10).WithMessage("BanksAccountNumber must be exactly 10 digits.");

            RuleFor(x => x.BanksAccountName)
                .NotEmpty().WithMessage("BanksAccountName is required.")
                .MaximumLength(100);

            RuleFor(x => x.BanksBvn)
                .NotEmpty().WithMessage("BanksBvn is required.")
                .Length(11).WithMessage("BanksBvn must be exactly 11 digits.");

            RuleFor(x => x.DestinationInstitutionCode)
                .NotEmpty().WithMessage("DestinationInstitutionCode is required.")
                .MaximumLength(20);

            RuleFor(x => x.SourceInstitutionCode)
                .NotEmpty().WithMessage("SourceInstitutionCode is required.")
                .MaximumLength(20);

            RuleFor(x => x.Narration)
                .NotEmpty().WithMessage("Narration is required.")
                .MaximumLength(250);

            RuleFor(x => x.MandateType)
                .GreaterThanOrEqualTo(0).WithMessage("MandateType is required.");

            RuleFor(x => x.MandateRequestStatus)
                .IsInEnum().WithMessage("MandateRequestStatus must be a valid enum value.");

            RuleFor(x => x.StartDate)
                .GreaterThan(DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("StartDate must be greater than today.");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("EndDate is required.")
                .GreaterThan(x => x.StartDate).WithMessage("EndDate must be after StartDate.");

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Location is required.")
                .MaximumLength(200);

            RuleFor(x => x.PaymentFrequency)
                .NotEmpty().WithMessage("PaymentFrequency is required.")
                .IsInEnum().WithMessage("PaymentFrequency must be a valid enum value.");
        }
    }
}

namespace CityCode.MandateSystem.Application.CommandHandlers
{
    public class EditMandateCommandHandler : IRequestHandler<EditMandateCommand, Common.Models.View.Result<MandateRequest>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly SystemSettings _systemSettings;

        public EditMandateCommandHandler(IApplicationDbContext context, IMapper mapper, IOptions<SystemSettings> systemSettings)
        {
            _context = context;
            _mapper = mapper;
            _systemSettings = systemSettings.Value;
        }

        public async Task<Common.Models.View.Result<MandateRequest>> Handle(EditMandateCommand request, CancellationToken cancellationToken)
        {
            var mandateRequest = await _context.MandateRequests
                .FirstOrDefaultAsync(x => x.MandateRequestId == request.MandateRequestId, cancellationToken);

            if (mandateRequest == null)
            {
                return Common.Models.View.Result<MandateRequest>.Failure(DateTime.UtcNow, "Mandate request not found.");
            }
            _mapper.Map(request, mandateRequest);

            mandateRequest.SetInitiatorDetails(request.InitiatedBy, request.InitiatedById);

            // Add domain event for activity log
            mandateRequest.AddDomainEvent(new ActivityLogEvent(new Activity
            {
                Action = "Updated Mandate",
                DateCreated = DateTime.UtcNow,
                Entity = "MandateRequest"
            }));

            // EF Core will track the changes and only update modified properties
            await _context.SaveChangesAsync(cancellationToken);

            return Common.Models.View.Result<MandateRequest>.Success(DateTime.UtcNow, mandateRequest);
        }
    }
}