using HSE_Bank.Domain.Factories;
using HSE_Bank.InMemory.Ids;
using HSE_Bank.InMemory.State;

namespace HSE_Bank.Application.Seeds;

public sealed class DemoDataBuilder
{
    private readonly AppState _state;
    private readonly IFinanceFactory _factory;
    private readonly IIdGenerator _ids;
    public DemoDataBuilder(AppState state, IFinanceFactory factory, IIdGenerator ids)
    { _state = state; _factory = factory; _ids = ids; }


    public DemoDataBuilder AddAccount(string name, out Guid id, decimal startBalance = 0m)
    {
        var acc = _factory.CreateAccount(_ids.New(), name, startBalance);
        _state.Accounts.Add(acc);
        id = acc.Id;
        return this;
    }


    public DemoDataBuilder AddCategory(string name, out Guid id, Guid? parent = null)
    {
        var cat = _factory.CreateCategory(_ids.New(), name, parent);
        _state.Categories.Add(cat);
        id = cat.Id;
        return this;
    }


    public DemoDataBuilder AddOperation(Guid acc, Guid cat, Domain.ValueObjects.OperationType type, decimal amount, DateTime date, string? desc = null)
    {
        var op = _factory.CreateOperation(acc, cat, type, amount, date, desc);
        _state.Operations.Add(op);
        var accEnt = _state.Accounts.Find(a => a.Id == acc)!;
        accEnt.Apply(op.SignedAmount);
        return this;
    }


    public void Seed()
    {
        AddAccount("Карта HSE Bank", out var acc1, 12000m)
            .AddAccount("Наличные", out var cash, 3000m)
            .AddCategory("Еда", out var food)
            .AddCategory("Кафе", out var cafe, food)
            .AddCategory("Зарплата", out var salary)
            .AddOperation(acc1, cafe, Domain.ValueObjects.OperationType.Expense, 450m, DateTime.Today.AddDays(-2), "Latte & croissant")
            .AddOperation(acc1, salary, Domain.ValueObjects.OperationType.Income, 120000m, DateTime.Today.AddDays(-1), "Salary")
            .AddOperation(cash, cafe, Domain.ValueObjects.OperationType.Expense, 800m, DateTime.Today, "Lunch");
    }
}