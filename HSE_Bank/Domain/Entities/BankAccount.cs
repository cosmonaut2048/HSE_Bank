namespace HSE_Bank.Domain.Entities;

public sealed class BankAccount
{
    public Guid Id { get; }
    public string Name { get; private set; }
    public decimal Balance { get; private set; }


    public BankAccount(Guid id, string name, decimal balance = 0m)
    {
        Id = id;
        Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Account name required") : name;
        Balance = balance;
    }


    public void Rename(string newName)
    {
        Name = string.IsNullOrWhiteSpace(newName) ? throw new ArgumentException("Account name required") : newName;
    }


    public void Apply(decimal delta)
    {
        Balance += delta;
    }
}
