namespace CityCode.MandateSystem.Application.Queries;

public class GetApiRequestLogsWithPaginationQueryValidator : AbstractValidator<GetApiRequestLogsWithPaginationQuery>
{
    public GetApiRequestLogsWithPaginationQueryValidator()
    {
        this.RuleFor(x => x.ApiRequestLogId)
            .NotEmpty().WithMessage("ListId is required.");

        this.RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber at least greater than or equal to 1.");

        this.RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");
    }
}
