using HSE_Bank.InMemory.State;

namespace HSE_Bank.Application.Analytics;

/// <summary>
/// Входные данные для стратегии анализа сумм за период.
/// Содержит диапазон дат для анализа.
/// </summary>
public sealed class SumByPeriodInput
{
    /// <summary>
    /// Начальная дата диапазона для анализа (включительно).
    /// </summary>
    public DateTime From { get; }
    
    /// <summary>
    /// Конечная дата диапазона для анализа (включительно).
    /// </summary>
    public DateTime To { get; }
    
    /// <summary>
    /// Конструктор для создания входных данных анализа за период.
    /// </summary>
    /// <param name="from">Начальная дата</param>
    /// <param name="to">Конечная дата</param>
    public SumByPeriodInput(DateTime from, DateTime to)
    {
        From = from;
        To = to;
    }
}

// Реализует паттерн Strategy.

/// <summary>
/// Стратегия для подсчета суммы всех операций (доходов и расходов) за выбранный период.
/// </summary>
public sealed class SumByPeriodStrategy : IAnalyticsStrategy<SumByPeriodInput, decimal>
{
    /// <summary>
    /// Состояние приложения, содержащее все операции в памяти.
    /// </summary>
    private readonly AppState _state;
    
    /// <summary>
    /// Конструктор стратегии.
    /// </summary>
    /// <param name="state">Состояние приложения для доступа к операциям</param>
    public SumByPeriodStrategy(AppState state) => _state = state;
    
    /// <summary>
    /// Выполняет подсчет суммы операций за указанный период.
    /// </summary>
    /// <param name="input">Входные данные с диапазоном дат для анализа</param>
    /// <param name="ct">Токен отмены для прерывания операции</param>
    /// <returns>Task с результатом - итоговой суммой операций за период</returns>
    public Task<decimal> CalculateAsync(SumByPeriodInput input, CancellationToken ct = default)
    {
        var sum = _state.Operations
            .Where(o => o.Date >= input.From && o.Date <= input.To)
            .Sum(o => o.SignedAmount);
        return Task.FromResult(sum);
    }
}