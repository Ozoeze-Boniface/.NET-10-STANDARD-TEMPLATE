using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityCode.MandateSystem.Application.Commands
{
    public class ApproveMandateCommand : IRequest<Common.Models.View.Result<Mandate>>
    {
        public long MandateId { get; set; }
        public long ApproverId { get; set; }
        public string ApprovedBy { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
    }

    public class ApproveMandateCommandValidator : AbstractValidator<ApproveMandateCommand>
    {
        public ApproveMandateCommandValidator()
        {
            RuleFor(x => x.MandateId)
                .GreaterThan(0).WithMessage("MandateId is required and must be greater than 0.");

            RuleFor(x => x.ApproverId)
                .GreaterThan(0).WithMessage("ApproverId is required and must be greater than 0.");

            RuleFor(x => x.ApprovedBy)
                .NotEmpty().WithMessage("ApprovedBy is required.")
                .MaximumLength(100);

            RuleFor(x => x.Comments)
                .NotEmpty().WithMessage("Comments are required.")
                .MaximumLength(500);
        }
    }
}