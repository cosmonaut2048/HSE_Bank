using HSE_Bank.Application.Commands.Abstractions;
using HSE_Bank.Domain.Factories;
using HSE_Bank.InMemory.Ids;
using HSE_Bank.InMemory.Repositories;

namespace HSE_Bank.Application.Commands.Account;

// Реализует паттерн Command Handler (часть паттерна Command).

/// <summary>
/// Обработчик команды CreateAccount.
/// Отвечает за всю логику создания банковского счета.
/// </summary>
public sealed class CreateAccountHandler : ICommandHandler<CreateAccount>
{
    // Реализует паттерн Factory.
    
    /// <summary>
    /// Фабрика доменных объектов для создания счетов с валидацией.
    /// </summary>
    private readonly IFinanceFactory _factory;
    
    /// <summary>
    /// Хранилище счетов для сохранения созданного счета.
    /// </summary>
    private readonly IAccountStore _accounts;
    
    /// <summary>
    /// Генератор уникальных ID для создания идентификатора счета.
    /// </summary>
    private readonly IIdGenerator _ids;
    
    /// <summary>
    /// Конструктор обработчика.
    /// </summary>
    /// <param name="factory">Фабрика для создания валидных объектов</param>
    /// <param name="accounts">Хранилище для сохранения счета</param>
    /// <param name="ids">Генератор ID</param>
    public CreateAccountHandler(IFinanceFactory factory, IAccountStore accounts, IIdGenerator ids)
    {
        _factory = factory;
        _accounts = accounts;
        _ids = ids;
    }
    
    /// <summary>
    /// Обрабатывает команду создания счета.
    /// </summary>
    /// <param name="cmd">Команда с данными для создания счета</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Завершенная задача</returns>
    public Task HandleAsync(CreateAccount cmd, CancellationToken ct = default)
    {
        // Генерируем новый ID
        var newId = _ids.New();
        
        // Создаём счет через фабрику
        var account = _factory.CreateAccount(newId, cmd.Name, cmd.StartBalance);
        
        // Сохраняем счет в хранилище
        _accounts.Add(account);
        
        // Устанавливаем ID в команду для возврата клиенту
        cmd.CreatedId = account.Id;
        
        // Возвращаем завершенную задачу
        return Task.CompletedTask;
    }
}