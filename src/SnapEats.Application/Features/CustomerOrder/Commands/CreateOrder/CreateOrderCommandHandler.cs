using MediatR;
using Microsoft.EntityFrameworkCore;
using SnapEats.Domain.Events;
using SnapEats.Domain.Exceptions;
using SnapEats.Domain.Interfaces;
using SnapEats.Infrastructure.Persistence;
using EntityCustomer = SnapEats.Infrastructure.Persistence.Entities.Customer;
using EntityOrder = SnapEats.Infrastructure.Persistence.Entities.CustomerOrder;
using EntityOrderItem = SnapEats.Infrastructure.Persistence.Entities.OrderItem;

namespace SnapEats.Application.Features.CustomerOrder.Commands.CreateOrder;


public sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
{
    private readonly ApplicationDbContext _context;
    private readonly IRealTimeNotificationService _notificationService;

    public CreateOrderCommandHandler(
        ApplicationDbContext context,
        IRealTimeNotificationService notificationService)
    {
        _context = context;
        _notificationService = notificationService;
    }

    public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        EntityCustomer? customer = null;

        if (request.CustomerId > 0)
        {
            customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.CustomerId == request.CustomerId, cancellationToken);
        }

        if (customer == null)
        {
            customer = await _context.Customers.FirstOrDefaultAsync(cancellationToken);
            if (customer == null)
            {
                customer = new EntityCustomer
                {
                    FullName = "عميل تطبيق الجوال",
                    Email = "customer@snapeats.com",
                    Phone = "0500000000",
                    PasswordHash = "DEFAULT_HASH"
                };
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        var order = new EntityOrder
        {
            CustomerId = customer.CustomerId,
            Status = "Pending",
            OrderDate = DateTime.UtcNow,
            OrderItems = new List<EntityOrderItem>()
        };

        foreach (var itemDto in request.Items)
        {
            var menuItem = await _context.MenuItems
                .FirstOrDefaultAsync(m => m.MenuItemId == itemDto.MenuItemId, cancellationToken);

            if (menuItem != null)
            {
                order.OrderItems.Add(new EntityOrderItem
                {
                    MenuItemId = itemDto.MenuItemId,
                    Quantity = itemDto.Quantity > 0 ? itemDto.Quantity : 1,
                    UnitPrice = menuItem.Price ?? 0
                });
            }
        }

        if (!order.OrderItems.Any())
        {
            throw new InvalidCategoryException("Cannot create an order without valid menu items.");
        }

        order.TotalAmount = order.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice);

        _context.CustomerOrders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);

        // Send real-time notification after successful DB commit
        await _notificationService.NotifyOrderCreatedAsync(new OrderCreatedEvent(
            OrderId: order.OrderId,
            CustomerId: customer.CustomerId,
            CustomerName: customer.FullName,
            OrderDate: order.OrderDate ?? DateTime.UtcNow,
            TotalAmount: order.TotalAmount ?? 0,
            ItemCount: order.OrderItems.Count,
            Status: order.Status ?? "Pending"
        ), cancellationToken);


        return order.OrderId;
    }
}

