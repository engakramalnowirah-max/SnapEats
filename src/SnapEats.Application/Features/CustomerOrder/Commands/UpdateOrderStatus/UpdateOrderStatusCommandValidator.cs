using FluentValidation;

namespace SnapEats.Application.Features.CustomerOrder.Commands.UpdateOrderStatus;

public sealed class UpdateOrderStatusCommandValidator : AbstractValidator<UpdateOrderStatusCommand>
{
    private static readonly string[] ValidStatuses =
        ["Pending", "Confirmed", "Preparing", "OutForDelivery", "Delivered", "Cancelled"];

    public UpdateOrderStatusCommandValidator()
    {
        RuleFor(v => v.OrderId)
            .GreaterThan(0);

        RuleFor(v => v.Status)
            .NotEmpty()
            .Must(s => ValidStatuses.Contains(s, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Invalid order status. Valid values: Pending, Confirmed, Preparing, OutForDelivery, Delivered, Cancelled");
    }
}

