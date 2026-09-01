using MediatR;
using Microsoft.EntityFrameworkCore;
using SnapEats.Domain.Events;
using SnapEats.Domain.Exceptions;
using SnapEats.Domain.Interfaces;
using SnapEats.Infrastructure.Persistence;

namespace SnapEats.Application.Features.CustomerOrder.Commands.DeleteOrder;

public sealed class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
{
    private readonly ApplicationDbContext _context;
    private readonly IRealTimeNotificationService _notificationService;

    public DeleteOrderCommandHandler(
        ApplicationDbContext context,
        IRealTimeNotificationService notificationService)
    {
        _context = context;
        _notificationService = notificationService;
    }

    public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _context.CustomerOrders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.OrderId == request.Id, cancellationToken)
            ?? throw new InvalidOrderException($"Order with Id '{request.Id}' not found.");

        var customerId = order.CustomerId;

        if (order.OrderItems != null && order.OrderItems.Any())
        {
            _context.OrderItems.RemoveRange(order.OrderItems);
        }

        _context.CustomerOrders.Remove(order);
        await _context.SaveChangesAsync(cancellationToken);

        // Notify real-time clients after successful DB delete
        await _notificationService.NotifyOrderDeletedAsync(new OrderDeletedEvent(
            OrderId: request.Id,
            CustomerId: customerId,
            DeletedAt: DateTime.UtcNow
        ), cancellationToken);
    }
}

