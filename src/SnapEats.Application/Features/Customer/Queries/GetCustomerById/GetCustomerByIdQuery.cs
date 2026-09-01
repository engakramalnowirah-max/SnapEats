using MediatR;
using SnapEats.Application.Features.Customer.DTOs;

namespace SnapEats.Application.Features.Customer.Queries.GetCustomerById;

public sealed record GetCustomerByIdQuery : IRequest<CustomerDto>
{
    public int Id { get; init; }
}

