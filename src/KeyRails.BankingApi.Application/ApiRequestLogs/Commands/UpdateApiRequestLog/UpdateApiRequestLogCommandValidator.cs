namespace KeyRails.BankingApi.Application.Commands;

public class UpdateApiRequestLogCommandValidator : AbstractValidator<UpdateApiRequestLogCommand>
{
    public UpdateApiRequestLogCommandValidator() => this.RuleFor(v => v.ApiRequestLogId)
             .NotNull();

}
