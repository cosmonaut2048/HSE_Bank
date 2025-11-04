using HSE_Bank.Domain.Entities;

namespace HSE_Bank.Application.Exporting.Visitor;

// Реализует паттерн Visitor

/// <summary>
/// Интерфейс посетителя для экспорта данных.
/// </summary>
public interface IExportVisitor
{
    /// <summary>
    /// Посещение банковского счета.
    /// Вызывается когда нужно экспортировать счет в нужном формате.
    /// </summary>
    /// <param name="acc">Объект счета для экспорта</param>
    void Visit(BankAccount acc);
    
    /// <summary>
    /// Посещение категории.
    /// Вызывается когда нужно экспортировать категорию в нужном формате.
    /// 
    /// ParentId может быть null (для корневых категорий) или содержать ID родительской категории.
    /// </summary>
    /// <param name="cat">Объект категории для экспорта</param>
    void Visit(Category cat);
    
    /// <summary>
    /// Посещение операции.
    /// Вызывается когда нужно экспортировать операцию в нужном формате.
    /// </summary>
    /// <param name="op">Объект операции для экспорта</param>
    void Visit(Operation op);
    
    /// <summary>
    /// Получить результат экспорта.
    /// Вызывается после посещения всех нужных объектов.
    /// 
    /// Может быть:
    /// - CSV
    /// - JSON
    /// - YAML
    /// - XML
    /// </summary>
    /// <returns>Строка с результатом экспорта в формате визитора</returns>
    string GetResult();
}