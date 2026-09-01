namespace SnapEats.Domain.ValueObjects;

public sealed record Quantity
{
    private const int MinQuantity = 1;
    private const int MaxQuantity = 999;

    private Quantity(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static Quantity Create(int value)
    {
        if (value < MinQuantity || value > MaxQuantity)
            throw new Domain.Exceptions.InvalidQuantityException(value);

        return new Quantity(value);
    }

    public static implicit operator int(Quantity quantity) => quantity.Value;

    public override string ToString() => Value.ToString();
}

