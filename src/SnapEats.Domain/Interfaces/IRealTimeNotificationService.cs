namespace SnapEats.Domain.Interfaces;

using SnapEats.Domain.Events;

public interface IRealTimeNotificationService
{
    Task NotifyOrderCreatedAsync(OrderCreatedEvent notification, CancellationToken cancellationToken = default);
    Task NotifyOrderStatusChangedAsync(OrderStatusChangedEvent notification, CancellationToken cancellationToken = default);
    Task NotifyOrderCancelledAsync(OrderCancelledEvent notification, CancellationToken cancellationToken = default);
    Task NotifyOrderDeletedAsync(OrderDeletedEvent notification, CancellationToken cancellationToken = default);
    Task NotifyCategoryChangedAsync(CategoryChangedEvent notification, CancellationToken cancellationToken = default);
    Task NotifyMenuItemChangedAsync(MenuItemChangedEvent notification, CancellationToken cancellationToken = default);
    Task NotifyDashboardUpdatedAsync(DashboardUpdatedEvent notification, CancellationToken cancellationToken = default);
}
