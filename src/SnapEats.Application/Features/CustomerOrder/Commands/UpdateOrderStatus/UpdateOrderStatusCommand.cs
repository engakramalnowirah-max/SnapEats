using MediatR;

namespace SnapEats.Application.Features.CustomerOrder.Commands.UpdateOrderStatus;

public sealed record UpdateOrderStatusCommand : IRequest
{
    public int OrderId { get; init; }
    public string Status { get; init; } = string.Empty;
}

