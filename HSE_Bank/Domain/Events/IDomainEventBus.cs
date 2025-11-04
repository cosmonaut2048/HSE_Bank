namespace HSE_Bank.Domain.Events;

public interface IDomainEventBus
{
    void Publish(IDomainEvent evt);
    void Subscribe<T>(Action<T> handler) where T : IDomainEvent;
}


public sealed class InMemoryDomainEventBus : IDomainEventBus
{
    private readonly Dictionary<System.Type, System.Delegate> _handlers = new();
    public void Publish(IDomainEvent evt)
    {
        var t = evt.GetType();
        if (_handlers.TryGetValue(t, out var del)) del.DynamicInvoke(evt);
    }
    public void Subscribe<T>(System.Action<T> handler) where T : IDomainEvent
    { _handlers[typeof(T)] = handler; }
}
