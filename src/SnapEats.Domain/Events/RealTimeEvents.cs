namespace SnapEats.Domain.Events;

public sealed record OrderCreatedEvent(
    int OrderId,
    int CustomerId,
    string CustomerName,
    DateTime OrderDate,
    decimal TotalAmount,
    int ItemCount,
    string Status = "Pending"
);

public sealed record OrderStatusChangedEvent(
    int OrderId,
    int CustomerId,
    string PreviousStatus,
    string NewStatus,
    DateTime UpdatedAt,
    string? Note = null
);

public sealed record OrderCancelledEvent(
    int OrderId,
    int CustomerId,
    string Reason,
    DateTime CancelledAt
);

public sealed record OrderDeletedEvent(
    int OrderId,
    int CustomerId,
    DateTime DeletedAt
);

public sealed record CategoryChangedEvent(
    string Action, // "Created", "Updated", "Deleted"
    int CategoryId,
    string Name,
    string? Description = null
);

public sealed record MenuItemChangedEvent(
    string Action, // "Created", "Updated", "Deleted"
    int MenuItemId,
    string Name,
    decimal Price,
    int CategoryId,
    bool IsAvailable = true
);

public sealed record DashboardUpdatedEvent(
    int TotalOrders,
    int PendingOrders,
    int CompletedOrders,
    decimal TotalRevenue,
    DateTime Timestamp
);
