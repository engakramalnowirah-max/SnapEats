namespace SnapEats.Domain.Exceptions;

public sealed class InvalidQuantityException : DomainException
{
    public InvalidQuantityException(int quantity)
        : base($"Quantity '{quantity}' is invalid. Quantity must be between 1 and 999.")
    {
        Quantity = quantity;
    }

    public int Quantity { get; }
}
