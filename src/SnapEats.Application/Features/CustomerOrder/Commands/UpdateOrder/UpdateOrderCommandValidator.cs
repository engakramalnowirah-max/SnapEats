using FluentValidation;

namespace SnapEats.Application.Features.CustomerOrder.Commands.UpdateOrder;

public sealed class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(v => v.Id)
            .GreaterThan(0);

        RuleFor(v => v.CustomerId)
            .GreaterThan(0);

        RuleFor(v => v.Status)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(v => v.TotalAmount)
            .GreaterThanOrEqualTo(0);
    }
}
