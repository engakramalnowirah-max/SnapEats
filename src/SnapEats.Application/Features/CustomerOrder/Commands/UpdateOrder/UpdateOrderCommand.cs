using MediatR;

namespace SnapEats.Application.Features.CustomerOrder.Commands.UpdateOrder;

public sealed record UpdateOrderCommand : IRequest
{
    public int Id { get; init; }
    public int CustomerId { get; init; }
    public string Status { get; init; } = string.Empty;
    public decimal TotalAmount { get; init; }
}
