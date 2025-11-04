using HSE_Bank.Domain.Entities;
using HSE_Bank.Domain.ValueObjects;

namespace HSE_Bank.Domain.Factories;

public interface IFinanceFactory
{
    Operation CreateOperation(Guid accId, Guid catId, OperationType type, decimal amount, DateTime date, string? desc);
    Entities.BankAccount CreateAccount(Guid id, string name, decimal startBalance = 0m);
    Entities.Category CreateCategory(Guid id, string name, Guid? parentId = null);
}

