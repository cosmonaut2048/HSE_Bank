namespace HSE_Bank.Application.Commands.Abstractions;

public interface ICommandHandler<T> where T : ICommand
{
    Task HandleAsync(T command, CancellationToken ct = default);
}