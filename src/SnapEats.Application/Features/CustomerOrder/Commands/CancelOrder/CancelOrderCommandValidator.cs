using FluentValidation;

namespace SnapEats.Application.Features.CustomerOrder.Commands.CancelOrder;

public sealed class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
{
    public CancelOrderCommandValidator()
    {
        RuleFor(v => v.OrderId)
            .GreaterThan(0);
    }
}

