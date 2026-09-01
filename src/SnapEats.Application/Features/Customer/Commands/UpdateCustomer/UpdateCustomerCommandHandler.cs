using MediatR;
using SnapEats.Domain.Exceptions;
using SnapEats.Infrastructure.Repositories;

namespace SnapEats.Application.Features.Customer.Commands.UpdateCustomer;

public sealed class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand>
{
    private readonly CustomerRepository _customerRepository;

    public UpdateCustomerCommandHandler(CustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new InvalidCustomerException($"Customer with Id '{request.Id}' not found.");

        customer.FullName = request.FullName;
        customer.Phone = request.Phone;

        await _customerRepository.UpdateAsync(customer, cancellationToken);
    }
}
