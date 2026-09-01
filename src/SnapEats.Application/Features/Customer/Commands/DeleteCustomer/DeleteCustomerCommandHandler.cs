using MediatR;
using Microsoft.EntityFrameworkCore;
using SnapEats.Domain.Exceptions;
using SnapEats.Infrastructure.Persistence;

namespace SnapEats.Application.Features.Customer.Commands.DeleteCustomer;

public sealed class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand>
{
    private readonly ApplicationDbContext _context;

    public DeleteCustomerCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers
            .Include(c => c.CustomerOrders)
                .ThenInclude(o => o.OrderItems)
            .FirstOrDefaultAsync(c => c.CustomerId == request.Id, cancellationToken)
            ?? throw new InvalidCustomerException($"Customer with Id '{request.Id}' not found.");

        if (customer.CustomerOrders != null && customer.CustomerOrders.Any())
        {
            foreach (var order in customer.CustomerOrders)
            {
                if (order.OrderItems != null && order.OrderItems.Any())
                {
                    _context.OrderItems.RemoveRange(order.OrderItems);
                }
            }
            _context.CustomerOrders.RemoveRange(customer.CustomerOrders);
        }

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
