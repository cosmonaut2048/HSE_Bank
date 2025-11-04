using HSE_Bank.Application.Commands.Abstractions;
using HSE_Bank.Domain.ValueObjects;

namespace HSE_Bank.Application.Commands.Operation;

// Реализует паттерн Command

/// <summary>
/// Команда для добавления финансовой операции (доход или расход).
/// </summary>
public sealed class AddOperation : ICommand
{
    /// <summary>
    /// ID банковского счета, к которому относится операция.
    /// </summary>
    public Guid AccountId { get; }
    
    /// <summary>
    /// ID категории операции.
    /// </summary>
    public Guid CategoryId { get; }
    
    /// <summary>
    /// Тип операции: Income (доход) или Expense (расход).
    /// </summary>
    public OperationType Type { get; }
    
    /// <summary>
    /// Сумма операции (всегда положительная).
    /// </summary>
    public decimal Amount { get; }
    
    /// <summary>
    /// Дата совершения операции.
    /// </summary>
    public DateTime Date { get; }
    
    /// <summary>
    /// Описание операции (необязательно).
    /// </summary>
    public string? Description { get; }
    
    /// <summary>
    /// Конструктор команды добавления операции.
    /// </summary>
    /// <param name="bankAccountId">ID счета</param>
    /// <param name="categoryId">ID категории</param>
    /// <param name="type">Тип операции (Income/Expense)</param>
    /// <param name="amount">Сумма (положительная)</param>
    /// <param name="date">Дата операции</param>
    /// <param name="description">Описание (необязательно)</param>
    public AddOperation(
        Guid bankAccountId,
        Guid categoryId,
        OperationType type,
        decimal amount,
        DateTime date,
        string? description = null)
    {
        AccountId = bankAccountId;
        CategoryId = categoryId;
        Type = type;
        Amount = amount;
        Date = date;
        Description = description;
    }
    
    /// <summary>
    /// ID созданной операции.
    /// </summary>
    public Guid CreatedId { get; internal set; }
}
