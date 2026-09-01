namespace SnapEats.Domain.Exceptions;

public sealed class DeliveredOrderException : DomainException
{
    public DeliveredOrderException(int orderId)
        : base($"Order with Id '{orderId}' has already been delivered and cannot be modified.")
    {
        OrderId = orderId;
    }

    public int OrderId { get; }
}
