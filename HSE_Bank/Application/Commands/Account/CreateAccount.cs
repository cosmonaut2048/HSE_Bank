using HSE_Bank.Application.Commands.Abstractions;

namespace HSE_Bank.Application.Commands.Account;

// Реализует паттерн Command.

/// <summary>
/// Команда для создания нового банковского счета.
/// </summary>
public sealed class CreateAccount : ICommand
{
    /// <summary>
    /// Название счета.
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Начальный баланс счета. По умолчанию 0.
    /// </summary>
    public decimal StartBalance { get; }
    
    /// <summary>
    /// Конструктор команды создания счета.
    /// </summary>
    /// <param name="name">Название счета</param>
    /// <param name="startBalance">Начальный баланс (по умолчанию 0)</param>
    public CreateAccount(string name, decimal startBalance = 0m)
    {
        Name = name;
        StartBalance = startBalance;
    }
    
    /// <summary>
    /// ID созданного счета. Устанавливается обработчиком команды после создания.
    /// </summary>
    public Guid CreatedId { get; internal set; }
}