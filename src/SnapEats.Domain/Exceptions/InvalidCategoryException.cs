namespace SnapEats.Domain.Exceptions;

public sealed class InvalidCategoryException : DomainException
{
    public InvalidCategoryException(string message)
        : base(message)
    {
    }
}
