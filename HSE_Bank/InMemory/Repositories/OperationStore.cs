using HSE_Bank.Domain.Entities;
using HSE_Bank.InMemory.State;

namespace HSE_Bank.InMemory.Repositories;

public sealed class OperationStore : IOperationStore
{
    private readonly AppState _state;
    public OperationStore(AppState s) => _state = s;
    public Operation? Get(Guid id) => _state.Operations.FirstOrDefault(o => o.Id == id);
    public IReadOnlyList<Operation> All() => _state.Operations;
    public void Add(Operation op) => _state.Operations.Add(op);
    public void Remove(Guid id) => _state.Operations.RemoveAll(o => o.Id == id);
}