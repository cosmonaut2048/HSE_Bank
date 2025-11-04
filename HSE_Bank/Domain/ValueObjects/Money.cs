namespace HSE_Bank.Domain.ValueObjects;

public readonly record struct Money(decimal Value)
{
    public static implicit operator decimal(Money m) => m.Value;
    public static implicit operator Money(decimal v) => new(v);
    public override string ToString() => Value.ToString("0.00");
}
