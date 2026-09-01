using FluentValidation;

namespace SnapEats.Application.Features.Category.Commands.CreateCategory;

public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(100);

        RuleFor(v => v.Description)
            .MaximumLength(500)
            .When(v => v.Description is not null);
    }
}

