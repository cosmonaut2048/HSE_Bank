namespace HSE_Bank.Domain.Events;

public sealed class OperationAdded : IDomainEvent
{
    public Guid OperationId { get; }
    public Guid AccountId { get; }
    public OperationAdded(Guid opId, Guid accId) { OperationId = opId; AccountId = accId; }
}
