using System.Text;
using HSE_Bank.Domain.Entities;

namespace HSE_Bank.Application.Exporting.Visitor;

public sealed class YamlExportVisitor : IExportVisitor
{
    private readonly StringBuilder _sb = new();
    public void Visit(BankAccount acc) => _sb.AppendLine($"- kind: account\n id: {acc.Id}\n name: {acc.Name}\n balance: {acc.Balance:0.00}");
    public void Visit(Category cat) => _sb.AppendLine($"- kind: category\n id: {cat.Id}\n parentId: {cat.ParentId}\n name: {cat.Name}");
    public void Visit(Operation op) => _sb.AppendLine($"- kind: operation\n id: {op.Id}\n accountId: {op.BankAccountId}\n categoryId: {op.CategoryId}\n type: {op.Type}\n amount: {op.Amount:0.00}\n date: {op.Date:yyyy-MM-dd}\n description: {op.Description}");
    public string GetResult() => _sb.ToString();
}