using MediatR;
using SnapEats.Domain.Events;
using SnapEats.Domain.Exceptions;
using SnapEats.Domain.Interfaces;
using SnapEats.Infrastructure.Repositories;

namespace SnapEats.Application.Features.CustomerOrder.Commands.UpdateOrder;

public sealed class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
{
    private readonly OrderRepository _orderRepository;
    private readonly CustomerRepository _customerRepository;
    private readonly IRealTimeNotificationService _notificationService;

    public UpdateOrderCommandHandler(
        OrderRepository orderRepository,
        CustomerRepository customerRepository,
        IRealTimeNotificationService notificationService)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _notificationService = notificationService;
    }

    public async Task Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken)
            ?? throw new InvalidOrderException($"Customer with Id '{request.CustomerId}' not found.");

        var order = await _orderRepository.GetOrderWithItemsAsync(request.Id, cancellationToken)
            ?? throw new InvalidOrderException($"Order with Id '{request.Id}' not found.");

        var previousStatus = order.Status ?? "Pending";
        order.CustomerId = request.CustomerId;
        order.Status = request.Status;
        order.TotalAmount = request.TotalAmount;

        await _orderRepository.UpdateAsync(order, cancellationToken);

        await _notificationService.NotifyOrderStatusChangedAsync(new OrderStatusChangedEvent(
            OrderId: order.OrderId,
            CustomerId: order.CustomerId,
            PreviousStatus: previousStatus,
            NewStatus: order.Status ?? "Pending",
            UpdatedAt: DateTime.UtcNow
        ), cancellationToken);
    }
}

