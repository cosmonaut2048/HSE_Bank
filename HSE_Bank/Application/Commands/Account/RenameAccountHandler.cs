using HSE_Bank.Application.Commands.Abstractions;
using HSE_Bank.InMemory.Repositories;

namespace HSE_Bank.Application.Commands.Account;

// Реализует паттерн Command Handler (часть паттерна Command).

/// <summary>
/// Обработчик команды RenameAccount.
/// Отвечает за логику переименования существующего счета.
/// </summary>
public sealed class RenameAccountHandler : ICommandHandler<RenameAccount>
{
    /// <summary>
    /// Хранилище счетов для получения и обновления счета.
    /// </summary>
    private readonly IAccountStore _accounts;
    
    /// <summary>
    /// Конструктор обработчика.
    /// </summary>
    /// <param name="accounts">Хранилище счетов</param>
    public RenameAccountHandler(IAccountStore accounts)
    {
        _accounts = accounts;
    }
    
    /// <summary>
    /// Обрабатывает команду переименования счета.
    /// </summary>
    /// <param name="cmd">Команда с ID счета и новым названием</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Завершенная задача</returns>
    public Task HandleAsync(RenameAccount cmd, CancellationToken ct = default)
    {
        // Получаем счет или выбрасываем исключение если не найден
        var acc = _accounts.Get(cmd.AccountId) 
            ?? throw new ArgumentException($"Счет с ID {cmd.AccountId} не найден");
        
        // Изменяем название счета (вызов метода доменного объекта)
        acc.Rename(cmd.NewName);
        
        // Сохраняем изменения в хранилище
        _accounts.Update(acc);
        
        // Возвращаем завершенную задачу
        return Task.CompletedTask;
    }
}