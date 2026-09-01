using FluentValidation;

namespace SnapEats.Application.Features.MenuItem.Commands.UpdateMenuItem;

public sealed class UpdateMenuItemCommandValidator : AbstractValidator<UpdateMenuItemCommand>
{
    public UpdateMenuItemCommandValidator()
    {
        RuleFor(v => v.Id)
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

