using FluentValidation;

namespace SnapEats.Application.Features.CustomerOrder.Commands.CreateOrder;

public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(v => v.CustomerId)
            .GreaterThan(0);

        RuleFor(v => v.Items)
            .NotEmpty()
            .WithMessage("Order must have at least one item.");

        RuleForEach(v => v.Items)
            .ChildRules(item =>
            {
                item.RuleFor(i => i.MenuItemId)
                    .GreaterThan(0);

                item.RuleFor(i => i.Quantity)
                    .GreaterThan(0)
                    .LessThanOrEqualTo(999);
            });
    }
}

