using HSE_Bank.Domain.Entities;

namespace HSE_Bank.InMemory.State;

public sealed class AppState
{
    public List<BankAccount> Accounts { get; } = new();
    public List<Category> Categories { get; } = new();
    public List<Operation> Operations { get; } = new();
}
