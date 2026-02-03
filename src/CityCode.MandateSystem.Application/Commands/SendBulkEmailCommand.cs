using CityCode.MandateSystem.Application.Dtos;
using CityCode.MandateSystem.Application.Services.UtilityServices.Interfaces;

namespace CityCode.MandateSystem.Application.Commands
{
    public class SendBulkEmailCommand : IRequest<Common.Models.View.Result<object>>
    {
        public List<string> Emails { get; set; } = new();
        public string Subject { get; set; } = string.Empty;
        public string Header { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }

    public class SendBulkEmailCommandValidator : AbstractValidator<SendBulkEmailCommand>
    {
        public SendBulkEmailCommandValidator()
        {
            RuleFor(x => x.Emails)
                .NotEmpty().WithMessage("Emails are required.");

            RuleForEach(x => x.Emails)
                .NotEmpty()
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Subject)
                .NotEmpty().WithMessage("Subject is required.");

            RuleFor(x => x.Header)
                .NotEmpty().WithMessage("Header is required.");

            RuleFor(x => x.Body)
                .NotEmpty().WithMessage("Body is required.");
        }
    }

    public class SendBulkEmailCommandHandler(IEmailService emailService)
        : IRequestHandler<SendBulkEmailCommand, Common.Models.View.Result<object>>
    {
        public Task<Common.Models.View.Result<object>> Handle(SendBulkEmailCommand request, CancellationToken cancellationToken)
        {
            var content = new MailContent
            {
                Header = request.Header,
                Subject = request.Subject,
                Body = request.Body
            };

            _ = Task.Run(async () =>
            {
                foreach (var email in request.Emails.Distinct(StringComparer.OrdinalIgnoreCase))
                {
                    await emailService.SendEmail(email.Trim(), content);
                }
            }, cancellationToken);

            return Task.FromResult(Common.Models.View.Result<object>.Success(
                DateTime.UtcNow,
                new { Count = request.Emails.Count },
                "Email dispatch queued"));
        }
    }
}
