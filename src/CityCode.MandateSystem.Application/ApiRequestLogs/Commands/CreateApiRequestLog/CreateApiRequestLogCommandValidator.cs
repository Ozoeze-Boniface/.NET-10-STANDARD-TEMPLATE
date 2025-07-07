namespace CityCode.MandateSystem.Application.Commands;

public class CreateApiRequestLogCommandValidator : AbstractValidator<CreateApiRequestLogCommand>
{
    public CreateApiRequestLogCommandValidator() => this.RuleFor(v => v.ApiRequestLogId)
            .NotNull();

}

