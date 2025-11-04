using HSE_Bank.Application.Commands.Abstractions;

namespace HSE_Bank.Application.Commands.Operation;

// Реализует паттерн Command для инкапсуляции запроса на удаление операции.

/// <summary>
/// Команда для удаления финансовой операции.
/// </summary>
public sealed class DeleteOperation : ICommand
{
    /// <summary>
    /// ID операции, которую нужно удалить.
    /// Если не существует - обработчик выбросит исключение.
    /// </summary>
    public Guid OperationId { get; }
    
    /// <summary>
    /// Конструктор команды удаления операции.
    /// </summary>
    /// <param name="operationId">ID операции для удаления</param>
    public DeleteOperation(Guid operationId)
    {
        OperationId = operationId;
    }
}