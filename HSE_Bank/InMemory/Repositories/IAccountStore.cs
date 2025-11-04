using HSE_Bank.Domain.Entities;

namespace HSE_Bank.InMemory.Repositories;

public interface IAccountStore
{
    BankAccount? Get(Guid id);
    IReadOnlyList<BankAccount> All();
    void Add(BankAccount acc);
    void Update(BankAccount acc);
}