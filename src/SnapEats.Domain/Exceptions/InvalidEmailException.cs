namespace SnapEats.Domain.Exceptions;

public sealed class InvalidEmailException : DomainException
{
    public InvalidEmailException(string email)
        : base($"Email '{email}' is not a valid email address.")
    {
        Email = email;
    }

    public string Email { get; }
}
