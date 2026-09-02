namespace SnapEats.API.Services;

using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SnapEats.API.Hubs;
using SnapEats.Domain.Events;
using SnapEats.Domain.Interfaces;

public sealed class SignalRNotificationService : IRealTimeNotificationService
{
    private readonly IHubContext<OrderHub> _hubContext;
    private readonly ILogger<SignalRNotificationService> _logger;

    public SignalRNotificationService(
        IHubContext<OrderHub> hubContext,
        ILogger<SignalRNotificationService> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task NotifyOrderCreatedAsync(OrderCreatedEvent notification, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Publishing OrderCreated event for OrderId: {OrderId}", notification.OrderId);
            var payload = new
            {
                orderId = notification.OrderId,
                customerId = notification.CustomerId,
                customerName = notification.CustomerName,
                orderDate = notification.OrderDate,
                totalAmount = notification.TotalAmount,
                itemCount = notification.ItemCount,
                status = notification.Status
            };
            await _hubContext.Clients.All.SendAsync("OrderCreated", payload, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send OrderCreated real-time notification for OrderId: {OrderId}", notification.OrderId);
        }
    }

    public async Task NotifyOrderStatusChangedAsync(OrderStatusChangedEvent notification, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Publishing OrderStatusChanged event for OrderId: {OrderId}, Status: {Status}", notification.OrderId, notification.NewStatus);
            var payload = new
            {
                orderId = notification.OrderId,
                customerId = notification.CustomerId,
                previousStatus = notification.PreviousStatus,
                newStatus = notification.NewStatus,
                updatedAt = notification.UpdatedAt,
                note = notification.Note
            };
            await _hubContext.Clients.All.SendAsync("OrderStatusChanged", payload, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send OrderStatusChanged real-time notification for OrderId: {OrderId}", notification.OrderId);
        }
    }

    public async Task NotifyOrderCancelledAsync(OrderCancelledEvent notification, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Publishing OrderCancelled event for OrderId: {OrderId}", notification.OrderId);
            var payload = new
            {
                orderId = notification.OrderId,
                customerId = notification.CustomerId,
                reason = notification.Reason,
                cancelledAt = notification.CancelledAt
            };
            await _hubContext.Clients.All.SendAsync("OrderCancelled", payload, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send OrderCancelled real-time notification for OrderId: {OrderId}", notification.OrderId);
        }
    }

    public async Task NotifyOrderDeletedAsync(OrderDeletedEvent notification, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Publishing OrderDeleted event for OrderId: {OrderId}", notification.OrderId);
            var payload = new
            {
                orderId = notification.OrderId,
                customerId = notification.CustomerId,
                deletedAt = notification.DeletedAt
            };
            await _hubContext.Clients.All.SendAsync("OrderDeleted", payload, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send OrderDeleted real-time notification for OrderId: {OrderId}", notification.OrderId);
        }
    }

    public async Task NotifyCategoryChangedAsync(CategoryChangedEvent notification, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Publishing CategoryChanged ({Action}) event for CategoryId: {CategoryId}", notification.Action, notification.CategoryId);
            var payload = new
            {
                action = notification.Action,
                categoryId = notification.CategoryId,
                name = notification.Name,
                description = notification.Description
            };
            await _hubContext.Clients.All.SendAsync("CategoryChanged", payload, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send CategoryChanged real-time notification for CategoryId: {CategoryId}", notification.CategoryId);
        }
    }

    public async Task NotifyMenuItemChangedAsync(MenuItemChangedEvent notification, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Publishing MenuItemChanged ({Action}) event for MenuItemId: {MenuItemId}", notification.Action, notification.MenuItemId);
            var payload = new
            {
                action = notification.Action,
                menuItemId = notification.MenuItemId,
                name = notification.Name,
                price = notification.Price,
                isAvailable = notification.IsAvailable,
                categoryId = notification.CategoryId
            };
            await _hubContext.Clients.All.SendAsync("MenuItemChanged", payload, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send MenuItemChanged real-time notification for MenuItemId: {MenuItemId}", notification.MenuItemId);
        }
    }

    public async Task NotifyDashboardUpdatedAsync(DashboardUpdatedEvent notification, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Publishing DashboardUpdated event");
            var payload = new
            {
                totalOrders = notification.TotalOrders,
                pendingOrders = notification.PendingOrders,
                completedOrders = notification.CompletedOrders,
                totalRevenue = notification.TotalRevenue,
                timestamp = notification.Timestamp
            };
            await _hubContext.Clients.All.SendAsync("DashboardUpdated", payload, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send DashboardUpdated real-time notification");
        }
    }
}
