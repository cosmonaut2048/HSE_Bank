namespace HSE_Bank.InMemory.Ids;

public sealed class GuidIdGenerator : IIdGenerator
{
    public Guid New() => Guid.NewGuid();
}