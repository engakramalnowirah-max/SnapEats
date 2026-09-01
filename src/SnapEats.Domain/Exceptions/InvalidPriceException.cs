namespace SnapEats.Domain.Exceptions;

public sealed class InvalidPriceException : DomainException
{
    public InvalidPriceException(decimal price)
        : base($"Price '{price:C}' is invalid. Price must be between 0.01 and 999,999.99.")
    {
        Price = price;
    }

    public decimal Price { get; }
}
