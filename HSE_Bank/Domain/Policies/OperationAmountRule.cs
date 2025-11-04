using HSE_Bank.Domain.ValueObjects;

namespace HSE_Bank.Domain.Policies;

public sealed class OperationDraft
{
    public System.Guid AccountId { get; }
    public System.Guid CategoryId { get; }
    public OperationType Type { get; }
    public decimal Amount { get; }
    public System.DateTime Date { get; }
    public string? Description { get; }
    public OperationDraft(System.Guid accId, System.Guid catId, OperationType type, decimal amount, System.DateTime date, string? description)
    { AccountId = accId; CategoryId = catId; Type = type; Amount = amount; Date = date; Description = description; }
}


public sealed class OperationAmountRule : IValidationRule<OperationDraft>
{
    public void Check(OperationDraft d)
    {
        if (d.Amount <= 0) throw new ArgumentException("Amount must be > 0");
        if (d.Date > System.DateTime.UtcNow.AddMinutes(5)) throw new ArgumentException("Date cannot be in future");
    }
}
