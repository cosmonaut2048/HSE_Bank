namespace HSE_Bank.Application.Commands.Abstractions;

public interface ICommandBus
{
    Task SendAsync<T>(T command, CancellationToken ct = default) where T : ICommand;
}