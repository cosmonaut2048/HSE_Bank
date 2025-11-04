using HSE_Bank.Application.Commands.Abstractions;

namespace HSE_Bank.Application.Commands.Account;

// Реализует паттерн Command.

/// <summary>
/// Команда для переименования существующего банковского счета.
/// </summary>
public sealed class RenameAccount : ICommand
{
    /// <summary>
    /// ID счета для переименования.
    /// </summary>
    public Guid AccountId { get; }
    
    /// <summary>
    /// Новое название для счета.
    /// </summary>
    public string NewName { get; }
    
    /// <summary>
    /// Конструктор команды переименования счета.
    /// </summary>
    /// <param name="accountId">ID счета для переименования</param>
    /// <param name="newName">Новое название счета</param>
    public RenameAccount(Guid accountId, string newName)
    {
        AccountId = accountId;
        NewName = newName;
    }
}