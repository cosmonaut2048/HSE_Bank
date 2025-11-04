using HSE_Bank.Domain.Entities;
using HSE_Bank.InMemory.State;

namespace HSE_Bank.InMemory.Repositories;

public sealed class CategoryStore : ICategoryStore
{
    private readonly AppState _state;
    public CategoryStore(AppState s) => _state = s;
    public Category? Get(Guid id) => _state.Categories.FirstOrDefault(c => c.Id == id);
    public IReadOnlyList<Category> All() => _state.Categories;
    public void Add(Category cat) => _state.Categories.Add(cat);
}