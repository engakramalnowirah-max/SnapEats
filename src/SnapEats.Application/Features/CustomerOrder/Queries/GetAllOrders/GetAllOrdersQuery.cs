using MediatR;
using SnapEats.Application.Common.Models;
using SnapEats.Application.Features.CustomerOrder.DTOs;

namespace SnapEats.Application.Features.CustomerOrder.Queries.GetAllOrders;

public sealed record GetAllOrdersQuery : IRequest<PagedResult<OrderDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public int? CustomerId { get; init; }
    public string? Status { get; init; }
}

