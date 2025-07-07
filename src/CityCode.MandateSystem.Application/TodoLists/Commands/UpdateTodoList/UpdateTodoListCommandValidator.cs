namespace CityCode.MandateSystem.Application.TodoLists.Commands.UpdateTodoList;
//using CityCode.MandateSystem.Application.Common.Interfaces;

public class UpdateTodoListCommandValidator : AbstractValidator<UpdateTodoListCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTodoListCommandValidator(IApplicationDbContext context)
    {
        this._context = context;

        this.RuleFor(v => v.Title)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(this.BeUniqueTitle)
                .WithMessage("'{PropertyName}' must be unique.")
                .WithErrorCode("Unique");
    }

    public async Task<bool> BeUniqueTitle(UpdateTodoListCommand model, string title, CancellationToken cancellationToken) => await this._context.TodoLists
            .Where(l => l.Id != model.Id)
            .AllAsync(l => l.Title != title, cancellationToken);
}
