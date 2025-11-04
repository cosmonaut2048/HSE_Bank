using HSE_Bank.Application.Commands.Abstractions;
using HSE_Bank.Application.Commands.Account;
using HSE_Bank.InMemory.Repositories;

namespace HSE_Bank.Application.Facades;

// Реализует паттерн Facade.

/// <summary>
/// Фасад для управления банковскими счетами.
/// </summary>
public sealed class AccountsFacade
{
    /// <summary>
    /// Шина команд для отправки команд на создание и изменение счетов.
    /// </summary>
    private readonly ICommandBus _bus;
    
    /// <summary>
    /// Хранилище счетов для чтения данных о счетах.
    /// </summary>
    private readonly IAccountStore _accounts;
    
    /// <summary>
    /// Конструктор фасада управления счетами.
    /// </summary>
    /// <param name="bus">Шина команд для отправки операций</param>
    /// <param name="accounts">Хранилище счетов</param>
    public AccountsFacade(ICommandBus bus, IAccountStore accounts)
    {
        _bus = bus;
        _accounts = accounts;
    }
    
    /// <summary>
    /// Асинхронно создает новый банковский счет.
    /// </summary>
    /// <param name="name">Название счета (например, "Карта Tinkoff")</param>
    /// <param name="startBalance">Начальный баланс счета, по умолчанию 0</param>
    /// <param name="ct">Токен отмены для прерывания операции</param>
    /// <returns>Task содержащий ID созданного счета</returns>
    public async Task<Guid> CreateAsync(string name, decimal startBalance = 0m, CancellationToken ct = default)
    {
        var cmd = new CreateAccount(name, startBalance);
        return await _bus.SendAsync(cmd, ct).ContinueWith(_ => cmd.CreatedId);
    }
    
    /// <summary>
    /// Получает список всех банковских счетов.
    /// </summary>
    /// <returns>Неизменяемый список всех счетов</returns>
    public IReadOnlyList<Domain.Entities.BankAccount> All() => _accounts.All();
    
    /// <summary>
    /// Асинхронно переименовывает существующий счет.
    /// Счет с заданным ID должен существовать.
    /// </summary>
    /// <param name="id">ID счета для переименования</param>
    /// <param name="newName">Новое название для счета</param>
    /// <param name="ct">Токен отмены для прерывания операции</param>
    /// <returns>Task, который завершится после переименования</returns>
    public Task RenameAsync(Guid id, string newName, CancellationToken ct = default)
        => _bus.SendAsync(new RenameAccount(id, newName), ct);
}
