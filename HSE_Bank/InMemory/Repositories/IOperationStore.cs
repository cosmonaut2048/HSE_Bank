using HSE_Bank.Domain.Entities;

namespace HSE_Bank.InMemory.Repositories;

public interface IOperationStore
{
    Operation? Get(Guid id);
    IReadOnlyList<Operation> All();
    void Add(Operation op);
    void Remove(Guid id);
}