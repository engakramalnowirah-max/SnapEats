using MediatR;
using SnapEats.Domain.Events;
using SnapEats.Domain.Exceptions;
using SnapEats.Domain.Interfaces;
using SnapEats.Infrastructure.Repositories;

namespace SnapEats.Application.Features.CustomerOrder.Commands.CancelOrder;


public sealed class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand>
{
    private readonly OrderRepository _orderRepository;
    private readonly IRealTimeNotificationService _notificationService;

    public CancelOrderCommandHandler(
        OrderRepository orderRepository,
        IRealTimeNotificationService notificationService)
    {
        _orderRepository = orderRepository;
        _notificationService = notificationService;
    }

    public async Task Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken)
            ?? throw new InvalidCategoryException($"Order with Id '{request.OrderId}' not found.");

        if (order.Status == "Delivered")
            throw new DeliveredOrderException(order.OrderId);

        if (order.Status != "Pending" && order.Status != "Preparing")
            throw new InvalidOperationException($"Cannot cancel order in status '{order.Status}'. Only Pending and Preparing orders can be cancelled.");

        order.Status = "Cancelled";
        await _orderRepository.UpdateAsync(order, cancellationToken);

        // Send real-time notification after DB update succeeds
        await _notificationService.NotifyOrderCancelledAsync(new OrderCancelledEvent(
            OrderId: order.OrderId,
            CustomerId: order.CustomerId,
            Reason: "Admin cancelled the order",
            CancelledAt: DateTime.UtcNow
        ), cancellationToken);


    }
}


