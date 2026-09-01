using MediatR;
using SnapEats.Application.Features.CustomerOrder.DTOs;

namespace SnapEats.Application.Features.CustomerOrder.Queries.GetOrderById;

public sealed record GetOrderByIdQuery : IRequest<OrderDetailDto>
{
    public int Id { get; init; }
}

