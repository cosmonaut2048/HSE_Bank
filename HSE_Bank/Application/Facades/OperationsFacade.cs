using HSE_Bank.Application.Commands.Abstractions;
using HSE_Bank.Application.Commands.Operation;
using HSE_Bank.InMemory.Repositories;

namespace HSE_Bank.Application.Facades;

// Реализует паттерн Facade.

/// <summary>
/// Фасад для управления финансовыми операциями (доходами и расходами).
/// </summary>
public sealed class OperationsFacade
{
    /// <summary>
    /// Шина команд для отправки команд добавления и удаления операций.
    /// </summary>
    private readonly ICommandBus _bus;
    
    /// <summary>
    /// Хранилище операций для чтения и получения списка операций.
    /// </summary>
    private readonly IOperationStore _ops;
    
    /// <summary>
    /// Конструктор фасада управления операциями.
    /// </summary>
    /// <param name="bus">Шина команд для отправки операций</param>
    /// <param name="ops">Хранилище операций</param>
    public OperationsFacade(ICommandBus bus, IOperationStore ops)
    {
        _bus = bus;
        _ops = ops;
    }
    
    /// <summary>
    /// Асинхронно добавляет новую финансовую операцию (доход или расход).
    /// </summary>
    /// <param name="accId">ID банковского счета, к которому относится операция</param>
    /// <param name="catId">ID категории операции (например, "Кафе", "Зарплата")</param>
    /// <param name="type">Тип операции: Income (доход) или Expense (расход)</param>
    /// <param name="amount">Сумма операции (должна быть положительной)</param>
    /// <param name="date">Дата совершения операции</param>
    /// <param name="desc">Описание операции (необязательное, может быть null)</param>
    /// <param name="ct">Токен отмены для прерывания операции</param>
    /// <returns>Task содержащий ID созданной операции</returns>
    public Task<Guid> AddAsync(Guid accId, Guid catId, Domain.ValueObjects.OperationType type, decimal amount, DateTime date, string? desc = null, CancellationToken ct = default)
    {
        var cmd = new AddOperation(accId, catId, type, amount, date, desc);
        return _bus.SendAsync(cmd, ct).ContinueWith(_ => cmd.CreatedId);
    }
    
    /// <summary>
    /// Асинхронно удаляет существующую финансовую операцию.
    ///
    /// Операция с заданным ID должна существовать.
    /// </summary>
    /// <param name="opId">ID операции для удаления</param>
    /// <param name="ct">Токен отмены для прерывания операции</param>
    /// <returns>Task, который завершится после удаления</returns>
    public Task DeleteAsync(Guid opId, CancellationToken ct = default)
        => _bus.SendAsync(new DeleteOperation(opId), ct);
    
    /// <summary>
    /// Получает список всех финансовых операций.
    /// </summary>
    /// <returns>Неизменяемый список всех операций</returns>
    public IReadOnlyList<Domain.Entities.Operation> All() => _ops.All();
}