using System.Text;
using HSE_Bank.Domain.Entities;

namespace HSE_Bank.Application.Exporting.Visitor;

// Реализует паттерн Visitor

/// <summary>
/// Visitor для экспорта данных в формат CSV.
/// </summary>
public sealed class CsvExportVisitor : IExportVisitor
{
    /// <summary>
    /// StringBuilder для накопления строк CSV результата.
    /// </summary>
    private readonly StringBuilder _sb = new();
    
    /// <summary>
    /// Метод для экспорта банковского счета в CSV.
    /// Формат: ACC;{Id};{Name};{Balance}
    /// </summary>
    /// <param name="acc">Объект счета для экспорта</param>
    public void Visit(BankAccount acc) => _sb.AppendLine($"ACC;{acc.Id};{acc.Name};{acc.Balance:0.00}");
    
    /// <summary>
    /// Метод для экспорта категории в CSV.
    /// Формат: CAT;{Id};{ParentId};{Name}
    /// </summary>
    /// <param name="cat">Объект категории для экспорта</param>
    public void Visit(Category cat) => _sb.AppendLine($"CAT;{cat.Id};{cat.ParentId};{cat.Name}");
    
    /// <summary>
    /// Метод для экспорта операции в CSV.
    /// Формат: OP;{Id};{BankAccountId};{CategoryId};{Type};{Amount};{Date};{Description}
    /// </summary>
    /// <param name="op">Объект операции для экспорта</param>
    public void Visit(Operation op) => _sb.AppendLine($"OP;{op.Id};{op.BankAccountId};{op.CategoryId};{op.Type};{op.Amount:0.00};{op.Date:yyyy-MM-dd};{op.Description}");
    
    /// <summary>
    /// Возвращает результат экспорта в формате CSV.
    /// Вызывается после посещения всех нужных объектов.
    /// </summary>
    /// <returns>Строка содержащая все экспортированные данные в CSV формате</returns>
    public string GetResult() => _sb.ToString();
}
