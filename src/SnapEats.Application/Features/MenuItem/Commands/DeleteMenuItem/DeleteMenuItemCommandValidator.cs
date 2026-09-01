using FluentValidation;

namespace SnapEats.Application.Features.MenuItem.Commands.DeleteMenuItem;

public sealed class DeleteMenuItemCommandValidator : AbstractValidator<DeleteMenuItemCommand>
{
    public DeleteMenuItemCommandValidator()
    {
        RuleFor(v => v.Id)
            .GreaterThan(0);
    }
}

