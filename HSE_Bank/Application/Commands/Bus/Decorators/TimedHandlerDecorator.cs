using System.Diagnostics;
using HSE_Bank.Application.Commands.Abstractions;

namespace HSE_Bank.Application.Commands.Bus.Decorators;

public sealed class TimedHandlerDecorator<T> : ICommandHandler<T> where T : ICommand
{
    private readonly ICommandHandler<T> _inner;
    private readonly Action<string,double> _report;
    public TimedHandlerDecorator(ICommandHandler<T> inner, Action<string,double> report)
    { _inner = inner; _report = report; }


    public async Task HandleAsync(T command, CancellationToken ct = default)
    {
        var sw = Stopwatch.StartNew();
        await _inner.HandleAsync(command, ct);
        sw.Stop();
        _report(typeof(T).Name, sw.Elapsed.TotalMilliseconds);
    }
}