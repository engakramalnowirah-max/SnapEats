using MediatR;

namespace SnapEats.Application.Features.CustomerOrder.Commands.CancelOrder;

public sealed record CancelOrderCommand : IRequest
{
    public int OrderId { get; init; }
}

