using HSE_Bank.Application.Commands.Abstractions;
using HSE_Bank.InMemory.Repositories;

namespace HSE_Bank.Application.Commands.Operation;

// Реализует паттерн Command Handler.

/// <summary>
/// Обработчик команды DeleteOperation.
/// Если вызвать удаление дважды - вернёт ошибку.
/// </summary>
public sealed class DeleteOperationHandler : ICommandHandler<DeleteOperation>
{
    /// <summary>
    /// Хранилище операций для получения и удаления операции.
    /// </summary>
    private readonly IOperationStore _operations;
    
    /// <summary>
    /// Хранилище счетов для получения и обновления счета.
    /// </summary>
    private readonly IAccountStore _accounts;
    
    /// <summary>
    /// Конструктор обработчика.
    /// </summary>
    /// <param name="operations">Хранилище операций</param>
    /// <param name="accounts">Хранилище счетов</param>
    public DeleteOperationHandler(IOperationStore operations, IAccountStore accounts)
    {
        _operations = operations;
        _accounts = accounts;
    }
    
    /// <summary>
    /// Обрабатывает команду удаления операции.
    /// </summary>
    /// <param name="cmd">Команда с ID операции для удаления</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Завершенная задача</returns>
    public Task HandleAsync(DeleteOperation cmd, CancellationToken ct = default)
    {
        // Получаем операцию или выбрасываем исключение
        var op = _operations.Get(cmd.OperationId)
            ?? throw new ArgumentException($"Операция {cmd.OperationId} не найдена");
        
        // Получаем счет операции или выбрасываем исключение
        var account = _accounts.Get(op.BankAccountId)
            ?? throw new ArgumentException($"Счет {op.BankAccountId} не найден");
        
        // Откатываем влияние операции на баланс
        account.Apply(-op.SignedAmount);
        
        // Сохраняем обновленный счет в хранилище
        _accounts.Update(account);
        
        // Удаляем операцию из хранилища
        _operations.Remove(op.Id);
        
        return Task.CompletedTask;
    }
}