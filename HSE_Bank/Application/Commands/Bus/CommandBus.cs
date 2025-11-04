using HSE_Bank.Application.Commands.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace HSE_Bank.Application.Commands.Bus;
// Реализует паттерн Command Bus

/// <summary>
/// Шина команд - для обработки всех команд в приложении.
/// </summary>
public sealed class CommandBus : ICommandBus
{
    /// <summary>
    /// Service Provider для получения обработчиков команд через DI контейнер.
    /// </summary>
    private readonly IServiceProvider _sp;
    
    /// <summary>
    /// Конструктор шины команд.
    /// </summary>
    /// <param name="sp">Service Provider для разрешения зависимостей</param>
    public CommandBus(IServiceProvider sp) => _sp = sp;
    
    /// <summary>
    /// Отправляет команду соответствующему обработчику асинхронно.
    /// Это позволяет новым командам добавляться без изменения шины.
    /// </summary>
    /// <typeparam name="T">Тип команды, которая должна быть обработана</typeparam>
    /// <param name="command">Экземпляр команды для обработки</param>
    /// <param name="ct">Токен отмены для преждевременного прерывания операции</param>
    /// <returns>Task, который завершится после обработки команды</returns>
    public Task SendAsync<T>(T command, CancellationToken ct = default) where T : ICommand
        => _sp.GetRequiredService<ICommandHandler<T>>().HandleAsync(command, ct);
}