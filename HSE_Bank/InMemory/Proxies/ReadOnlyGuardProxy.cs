using HSE_Bank.Domain.Entities;

namespace HSE_Bank.InMemory.Proxies;

public class ReadOnlyGuardProxy
{
    public interface IReadOnlyGuard { bool IsReadOnly { get; } }


    public sealed class ReadOnlyGuard : IReadOnlyGuard
    {
        public bool IsReadOnly { get; private set; }
        public void Enable() => IsReadOnly = true;
        public void Disable() => IsReadOnly = false;
    }


// Proxy над OperationStore для запрета модификаций в режиме read-only.
    public interface IOperationStoreRO
    {
        Operation? Get(Guid id);
        IReadOnlyList<Operation> All();
        void Add(Operation op);
        void Remove(Guid id);
    }
}