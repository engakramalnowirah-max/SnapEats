namespace SnapEats.Domain.Exceptions;

public sealed class InvalidNameException : DomainException
{
    public InvalidNameException(string name)
        : base($"Name '{name}' is invalid. Name must be between 2 and 100 characters.")
    {
        Name = name;
    }

    public string Name { get; }
}
