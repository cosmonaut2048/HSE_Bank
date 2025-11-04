using HSE_Bank.Domain.Entities;

namespace HSE_Bank.Application.Exporting.Visitor;

public sealed class JsonExportVisitor : IExportVisitor
{
    private readonly System.Text.Json.Nodes.JsonArray _arr = new();
    public void Visit(BankAccount acc)
    {
        var o = new System.Text.Json.Nodes.JsonObject
        {
            ["kind"] = "account",
            ["id"] = acc.Id.ToString(),
            ["name"] = acc.Name,
            ["balance"] = acc.Balance
        };
        _arr.Add(o);
    }
    public void Visit(Category cat)
    {
        var o = new System.Text.Json.Nodes.JsonObject
        {
            ["kind"] = "category",
            ["id"] = cat.Id.ToString(),
            ["parentId"] = cat.ParentId?.ToString(),
            ["name"] = cat.Name
        };
        _arr.Add(o);
    }
    public void Visit(Operation op)
    {
        var o = new System.Text.Json.Nodes.JsonObject
        {
            ["kind"] = "operation",
            ["id"] = op.Id.ToString(),
            ["accountId"] = op.BankAccountId.ToString(),
            ["categoryId"] = op.CategoryId.ToString(),
            ["type"] = op.Type.ToString(),
            ["amount"] = op.Amount,
            ["date"] = op.Date,
            ["description"] = op.Description
        };
        _arr.Add(o);
    }
    public string GetResult() => _arr.ToJsonString(new System.Text.Json.JsonSerializerOptions{WriteIndented = true});
}