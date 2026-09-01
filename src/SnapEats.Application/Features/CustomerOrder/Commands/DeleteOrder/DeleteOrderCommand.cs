using MediatR;

namespace SnapEats.Application.Features.CustomerOrder.Commands.DeleteOrder;

public sealed record DeleteOrderCommand : IRequest
{
    public int Id { get; init; }
}
