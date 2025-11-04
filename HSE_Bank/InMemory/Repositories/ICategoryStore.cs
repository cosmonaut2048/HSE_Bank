using HSE_Bank.Domain.Entities;

namespace HSE_Bank.InMemory.Repositories;

public interface ICategoryStore
{
    Category? Get(Guid id);
    IReadOnlyList<Category> All();
    void Add(Category cat);
}