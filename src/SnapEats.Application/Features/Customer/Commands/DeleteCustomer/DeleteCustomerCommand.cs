using MediatR;

namespace SnapEats.Application.Features.Customer.Commands.DeleteCustomer;

public sealed record DeleteCustomerCommand : IRequest
{
    public int Id { get; init; }
}
