namespace SnapEats.Domain.Exceptions;

public sealed class EmptyOrderException : DomainException
{
    public EmptyOrderException()
        : base("Cannot confirm an empty order. At least one order item is required.")
    {
    }
}
