using MediatR;
using SnapEats.Application.Common.Models;
using SnapEats.Application.Features.Customer.DTOs;

namespace SnapEats.Application.Features.Customer.Queries.GetAllCustomers;

public sealed record GetAllCustomersQuery : IRequest<PagedResult<CustomerDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SearchTerm { get; init; }
}

