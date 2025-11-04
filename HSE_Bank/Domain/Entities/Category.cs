namespace HSE_Bank.Domain.Entities;

public sealed class Category
{
    public Guid Id { get; }
    public string Name { get; }
    public Guid? ParentId { get; }


    public Category(Guid id, string name, Guid? parentId = null)
    {
        Id = id;
        Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Category name required") : name;
        ParentId = parentId;
    }
}
