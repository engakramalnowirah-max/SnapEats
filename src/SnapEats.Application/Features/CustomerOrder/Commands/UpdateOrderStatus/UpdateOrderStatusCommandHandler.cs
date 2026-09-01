using MediatR;
using SnapEats.Domain.Events;
using SnapEats.Domain.Exceptions;
using SnapEats.Domain.Interfaces;
using SnapEats.Infrastructure.Repositories;

namespace SnapEats.Application.Features.CustomerOrder.Commands.UpdateOrderStatus;


public sealed class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand>
{
    private readonly OrderRepository _orderRepository;
    private readonly IRealTimeNotificationService _notificationService;

    public UpdateOrderStatusCommandHandler(
        OrderRepository orderRepository,
        IRealTimeNotificationService notificationService)
    {
        _orderRepository = orderRepository;
        _notificationService = notificationService;
    }

    public async Task Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken)
            ?? throw new InvalidCategoryException($"Order with Id '{request.OrderId}' not found.");

        var previousStatus = order.Status ?? "Unknown";
        var newStatus = request.Status;

        // Validate status transitions
        if (!IsValidStatusTransition(previousStatus, newStatus))
        {
            throw new InvalidOperationException($"Invalid status transition from '{previousStatus}' to '{newStatus}'.");
        }

        order.Status = newStatus;
        await _orderRepository.UpdateAsync(order, cancellationToken);

        // Send real-time notification after DB update succeeds
        await _notificationService.NotifyOrderStatusChangedAsync(new OrderStatusChangedEvent(
            OrderId: order.OrderId,
            CustomerId: order.CustomerId,
            PreviousStatus: previousStatus,
            NewStatus: order.Status ?? "Unknown",
            UpdatedAt: DateTime.UtcNow
        ), cancellationToken);
    }

    private static bool IsValidStatusTransition(string from, string to)
    {
        return (from, to) switch
        {
            ("Pending", "Preparing") => true,
            ("Preparing", "Delivered") => true,
            ("Preparing", "OutForDelivery") => true,
            ("OutForDelivery", "Delivered") => true,
            ("Pending", "Cancelled") => true,
            ("Preparing", "Cancelled") => true,
            (_, "Cancelled") => false,
            (_, "Delivered") => false,
            _ => false
        };
    }
}


