using HSE_Bank.Domain.ValueObjects;

namespace HSE_Bank.Domain.Entities;

public sealed class Operation
{
    public Guid Id { get; }
    public Guid BankAccountId { get; }
    public Guid CategoryId { get; }
    public OperationType Type { get; }
    public decimal Amount { get; }
    public DateTime Date { get; }
    public string? Description { get; }


    public Operation(Guid id, Guid accId, Guid catId, OperationType type, decimal amount, DateTime date, string? description)
    {
        Id = id;
        BankAccountId = accId;
        CategoryId = catId;
        Type = type;
        Amount = amount;
        Date = date;
        Description = description;
    }


    public decimal SignedAmount => Type == OperationType.Income ? Amount : -Amount;
}
