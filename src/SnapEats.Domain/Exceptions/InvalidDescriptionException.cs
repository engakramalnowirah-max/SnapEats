namespace SnapEats.Domain.Exceptions;

public sealed class InvalidDescriptionException : DomainException
{
    public InvalidDescriptionException(string description)
        : base($"Description cannot exceed 500 characters. Provided: {description.Length} characters.")
    {
        Description = description;
    }

    public string Description { get; }
}
