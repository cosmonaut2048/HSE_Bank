using HSE_Bank.Application.Commands.Abstractions;
using HSE_Bank.Domain.Events;
using HSE_Bank.Domain.Factories;
using HSE_Bank.InMemory.Repositories;

namespace HSE_Bank.Application.Commands.Operation;

// Реализует паттерн Command Handler.

/// <summary>
/// Обработчик команды AddOperation.
/// </summary>
public sealed class AddOperationHandler : ICommandHandler<AddOperation>
{
    /// <summary>
    /// Фабрика для создания операций с валидацией.
    /// </summary>
    private readonly IFinanceFactory _factory;

    /// <summary>
    /// Хранилище операций для сохранения новой операции.
    /// </summary>
    private readonly IOperationStore _operations;

    /// <summary>
    /// Хранилище счетов для получения счета и обновления его баланса.
    /// </summary>
    private readonly IAccountStore _accounts;

    private readonly IDomainEventBus _bus;

    /// <summary>
    /// Конструктор обработчика.
    /// </summary>
    public AddOperationHandler(
        IFinanceFactory factory,
        IOperationStore operations,
        IAccountStore accounts,
        IDomainEventBus bus)
    {
        _factory = factory;
        _operations = operations;
        _accounts = accounts;
        _bus = bus;
    }

    /// <summary>
    /// Обрабатывает команду добавления операции.
    /// 
    /// Операция и счет обновляются оба:
    /// Если обновить только операцию - баланс счета не изменится.
    /// Если обновить только счет - данные будут несогласованными.
    /// </summary>
    /// <param name="cmd">Команда с данными операции</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Завершенная задача</returns>
    public Task HandleAsync(AddOperation cmd, CancellationToken ct = default)
    {
        var op = _factory.CreateOperation(cmd.AccountId, cmd.CategoryId, cmd.Type, cmd.Amount, cmd.Date,
            cmd.Description);
        _operations.Add(op);


        var acc = _accounts.Get(cmd.AccountId) ?? throw new ArgumentException("Account not found");
        acc.Apply(op.SignedAmount);
        _accounts.Update(acc);


        _bus.Publish(new OperationAdded(op.Id, acc.Id));
        cmd.CreatedId = op.Id;
        return Task.CompletedTask;
    }
}