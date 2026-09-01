using MediatR;
using SnapEats.Application.Common.Models;
using SnapEats.Domain.Exceptions;
using SnapEats.Infrastructure.Identity;
using SnapEats.Infrastructure.Repositories;

namespace SnapEats.Application.Features.Customer.Commands.RegisterCustomer;

public sealed class RegisterCustomerCommandHandler : IRequestHandler<RegisterCustomerCommand, AuthResult>
{
    private readonly CustomerRepository _customerRepository;
    private readonly PasswordService _passwordService;

    public RegisterCustomerCommandHandler(
        CustomerRepository customerRepository,
        PasswordService passwordService)
    {
        _customerRepository = customerRepository;
        _passwordService = passwordService;
    }

    public async Task<AuthResult> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
    {
        var emailUnique = await _customerRepository.IsEmailUniqueAsync(request.Email, cancellationToken);
        if (!emailUnique)
            throw new InvalidEmailException(request.Email);

        var passwordHash = _passwordService.HashPassword(request.Password);

        var customer = new SnapEats.Infrastructure.Persistence.Entities.Customer
        {
            FullName = request.FullName,
            Email = request.Email.ToLower(),
            Phone = request.Phone,
            PasswordHash = passwordHash
        };

        await _customerRepository.AddAsync(customer, cancellationToken);

        return new AuthResult(
            string.Empty,
            string.Empty,
            DateTime.UtcNow.AddHours(1),
            "Customer",
            customer.FullName,
            customer.Email,
            customer.CustomerId);
    }
}
