using HSE_Bank.Application.Commands.Abstractions;

namespace HSE_Bank.Application.Commands.Bus.Decorators;

public sealed class LoggingHandlerDecorator<T> : ICommandHandler<T> where T : ICommand
{
    private readonly ICommandHandler<T> _inner;
    private readonly Action<string> _log;
    public LoggingHandlerDecorator(ICommandHandler<T> inner, Action<string> log)
    { _inner = inner; _log = log; }


    public async Task HandleAsync(T command, CancellationToken ct = default)
    {
        _log($"Handling {typeof(T).Name}");
        await _inner.HandleAsync(command, ct);
        _log($"Handled {typeof(T).Name}");
    }
}