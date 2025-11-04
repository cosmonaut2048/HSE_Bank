using HSE_Bank.Domain.Entities;

namespace HSE_Bank.InMemory.State;

public interface IStateAccess
{
    IReadOnlyList<BankAccount> Accounts { get; }
    IReadOnlyList<Category> Categories { get; }
    IReadOnlyList<Operation> Operations { get; }
}


public sealed class StateAccess : IStateAccess
{
    private readonly AppState _state;
    public StateAccess(AppState state) => _state = state;
    public IReadOnlyList<BankAccount> Accounts => _state.Accounts;
    public IReadOnlyList<Category> Categories => _state.Categories;
    public IReadOnlyList<Operation> Operations => _state.Operations;
}
