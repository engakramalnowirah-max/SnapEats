using FluentValidation;

namespace SnapEats.Application.Features.MenuItem.Commands.CreateMenuItem;

public sealed class CreateMenuItemCommandValidator : AbstractValidator<CreateMenuItemCommand>
{
    public CreateMenuItemCommandValidator()
    {
        RuleFor(v => v.CategoryId)
            .GreaterThan(0);

        RuleFor(v => v.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(100);

        RuleFor(v => v.Description)
            .MaximumLength(500)
            .When(v => v.Description is not null);

        RuleFor(v => v.Price)
            .GreaterThan(0)
            .LessThanOrEqualTo(999999.99m);

        RuleFor(v => v.ImageUrl)
            .MaximumLength(500)
            .When(v => v.ImageUrl is not null);
    }
}

