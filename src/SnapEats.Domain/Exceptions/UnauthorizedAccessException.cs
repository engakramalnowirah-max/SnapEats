namespace SnapEats.Domain.Exceptions;

public sealed class UnauthorizedDomainAccessException : DomainException
{
    public UnauthorizedDomainAccessException(string message)
        : base(message)
    {
    }
}
