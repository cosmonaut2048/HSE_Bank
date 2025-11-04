using HSE_Bank.Domain.Entities;
using HSE_Bank.Domain.Policies;
using HSE_Bank.Domain.ValueObjects;
using HSE_Bank.InMemory.Ids;
using HSE_Bank.InMemory.State;

namespace HSE_Bank.Domain.Factories;

public sealed class ValidatingFinanceFactory : IFinanceFactory
{
    private readonly IEnumerable<IValidationRule<OperationDraft>> _rules;
    private readonly IIdGenerator _ids;
    public ValidatingFinanceFactory(IEnumerable<IValidationRule<OperationDraft>> rules, IIdGenerator ids)
    { _rules = rules; _ids = ids; }


    public Operation CreateOperation(Guid accId, Guid catId, OperationType type, decimal amount, DateTime date, string? desc)
    {
        var draft = new OperationDraft(accId, catId, type, amount, date, desc);
        foreach (var r in _rules) r.Check(draft);
        return new Operation(_ids.New(), accId, catId, type, amount, date, desc);
    }


    public BankAccount CreateAccount(Guid id, string name, decimal startBalance = 0m)
        => new(id, name, startBalance);


    public Category CreateCategory(Guid id, string name, Guid? parentId = null)
        => new(id, name, parentId);
}
