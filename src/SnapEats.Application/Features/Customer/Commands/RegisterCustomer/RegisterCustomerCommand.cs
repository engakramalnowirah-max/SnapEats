using MediatR;
using SnapEats.Application.Common.Models;

namespace SnapEats.Application.Features.Customer.Commands.RegisterCustomer;

public sealed record RegisterCustomerCommand : IRequest<AuthResult>
{
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

