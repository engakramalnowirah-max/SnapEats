namespace SnapEats.Domain.Exceptions;

public sealed class InvalidPhoneException : DomainException
{
    public InvalidPhoneException(string phone)
        : base($"Phone number '{phone}' is not valid.")
    {
        Phone = phone;
    }

    public string Phone { get; }
}
