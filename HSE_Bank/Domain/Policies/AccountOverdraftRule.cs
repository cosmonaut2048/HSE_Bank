using HSE_Bank.InMemory.State;

namespace HSE_Bank.Domain.Policies;

// Проверка на овердрафт: не уходим в минус ниже допустимого
public sealed class AccountOverdraftRule : IValidationRule<OperationDraft>
{
    private readonly AppState _state;
    private readonly decimal _overdraftLimit;
    public AccountOverdraftRule(AppState state, decimal overdraftLimit = -10_000m)
    { _state = state; _overdraftLimit = overdraftLimit; }


    public void Check(OperationDraft d)
    {
        var acc = _state.Accounts.FirstOrDefault(a => a.Id == d.AccountId)
                  ?? throw new ArgumentException("Account not found");
        var projected = acc.Balance + (d.Type == ValueObjects.OperationType.Income ? d.Amount : -d.Amount);
        if (projected < _overdraftLimit)
            throw new ArgumentException($"Overdraft limit exceeded: {projected}");
    }
}
