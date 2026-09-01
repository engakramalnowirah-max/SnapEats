using FluentValidation;

namespace SnapEats.Application.Features.CustomerOrder.Commands.DeleteOrder;

public sealed class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
{
    public DeleteOrderCommandValidator()
    {
        RuleFor(v => v.Id)
            .GreaterThan(0);
    }
}
