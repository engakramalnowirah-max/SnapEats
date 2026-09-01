namespace SnapEats.Domain.Exceptions;

public sealed class InvalidCustomerException : DomainException
{
    public InvalidCustomerException(string message)
        : base(message)
    {
    }
}
