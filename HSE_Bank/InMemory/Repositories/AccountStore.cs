using HSE_Bank.Domain.Entities;
using HSE_Bank.InMemory.State;

namespace HSE_Bank.InMemory.Repositories;

public sealed class AccountStore : IAccountStore
{
    private readonly AppState _state;
    public AccountStore(AppState s) => _state = s;
    public BankAccount? Get(Guid id) => _state.Accounts.FirstOrDefault(a => a.Id == id);
    public IReadOnlyList<BankAccount> All() => _state.Accounts;
    public void Add(BankAccount acc) => _state.Accounts.Add(acc);
    public void Update(BankAccount acc)
    {
        var idx = _state.Accounts.FindIndex(a => a.Id == acc.Id);
        if (idx >= 0) _state.Accounts[idx] = acc;
    }
}