using FluentValidation;

namespace SnapEats.Application.Features.Category.Commands.DeleteCategory;

public sealed class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(v => v.Id)
            .GreaterThan(0);
    }
}

