namespace CityCode.MandateSystem.Application.TodoLists.Commands.CreateTodoList;
using CityCode.MandateSystem.Application.Common.Interfaces;

public class CreateTodoListCommandValidator : AbstractValidator<CreateTodoListCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateTodoListCommandValidator(IApplicationDbContext context)
    {
        this._context = context;

        this.RuleFor(v => v.Title)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(this.BeUniqueTitle)
                .WithMessage("'{PropertyName}' must be unique.")
                .WithErrorCode("Unique");
    }

    public async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken) => await this._context.TodoLists
            .AllAsync(l => l.Title != title, cancellationToken);
}
