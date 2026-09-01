namespace SnapEats.Domain.ValueObjects;

public sealed record Price
{
    private const decimal MinPrice = 0.01m;
    private const decimal MaxPrice = 999_999.99m;

    private Price(decimal value)
    {
        Value = value;
    }

    public decimal Value { get; }

    public static Price Create(decimal value)
    {
        if (value < MinPrice || value > MaxPrice)
            throw new Domain.Exceptions.InvalidPriceException(value);

        return new Price(value);
    }

    public static implicit operator decimal(Price price) => price.Value;

    public override string ToString() => Value.ToString("F2");
}

