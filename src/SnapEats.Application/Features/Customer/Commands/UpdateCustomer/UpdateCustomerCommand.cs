using MediatR;

namespace SnapEats.Application.Features.Customer.Commands.UpdateCustomer;

public sealed record UpdateCustomerCommand : IRequest
{
    public int Id { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
}
