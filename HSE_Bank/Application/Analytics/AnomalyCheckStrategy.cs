using HSE_Bank.InMemory.State;

namespace HSE_Bank.Application.Analytics;

/// <summary>
/// Входные данные для стратегии проверки аномалий.
/// Содержит пороговое значение для определения аномалии.
/// </summary>
public sealed class AnomalyCheckInput
{
    /// <summary>
    /// Пороговое значение для определения аномалии.
    /// Операция считается аномалией если её сумма превышает этот порог.
    /// </summary>
    public decimal Threshold { get; }
    
    /// <summary>
    /// Конструктор входных данных.
    /// </summary>
    /// <param name="threshold">Пороговое значение</param>
    public AnomalyCheckInput(decimal threshold)
    {
        Threshold = threshold;
    }
}

// Реализует паттерн Strategy.
// Аномалия - это операция, сумма которой превышает установленный порог.

/// <summary>
/// Стратегия для проверки наличия аномалий в операциях.
/// </summary>
public sealed class AnomalyCheckStrategy : IAnalyticsStrategy<AnomalyCheckInput, bool>
{
    /// <summary>
    /// Состояние приложения с операциями.
    /// </summary>
    private readonly AppState _state;
    
    /// <summary>
    /// Конструктор стратегии.
    /// </summary>
    /// <param name="state">Состояние приложения</param>
    public AnomalyCheckStrategy(AppState state) => _state = state;
    
    /// <summary>
    /// Выполняет проверку наличия аномалий в операциях.
    /// </summary>
    /// <param name="input">Входные данные с пороговым значением</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Task с результатом - true если найдены аномалии, false если нет</returns>
    public Task<bool> CalculateAsync(AnomalyCheckInput input, CancellationToken ct = default)
    {
        var hasAnomalies = _state.Operations
            .Any(o => o.Amount > input.Threshold);  // Проверяем каждую операцию
        
        return Task.FromResult(hasAnomalies);
    }
}