using MediatR;
using SnapEats.Application.Features.CustomerOrder.DTOs;

namespace SnapEats.Application.Features.CustomerOrder.Commands.CreateOrder;

public sealed record CreateOrderCommand : IRequest<int>
{
    public int CustomerId { get; init; }
    public List<OrderItemRequestDto> Items { get; init; } = [];
}

public sealed record OrderItemRequestDto
{
    public int MenuItemId { get; init; }
    public int Quantity { get; init; }
}

