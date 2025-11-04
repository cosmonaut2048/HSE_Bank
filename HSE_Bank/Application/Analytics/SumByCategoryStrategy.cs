using HSE_Bank.InMemory.State;

namespace HSE_Bank.Application.Analytics;

/// <summary>
/// Входные данные для стратегии анализа сумм по категории.
/// Содержит ID категории, для которой нужно подсчитать сумму операций.
/// </summary>
public sealed class SumByCategoryInput
{
    /// <summary>
    /// ID категории, операции которой нужно анализировать.
    /// </summary>
    public Guid CategoryId { get; }
    
    /// <summary>
    /// Конструктор входных данных.
    /// </summary>
    /// <param name="categoryId">ID категории для анализа</param>
    public SumByCategoryInput(Guid categoryId)
    {
        CategoryId = categoryId;
    }
}

// Реализует паттерн Strategy.

/// <summary>
/// Стратегия для подсчета суммы всех операций по конкретной категории.
/// </summary>
public sealed class SumByCategoryStrategy : IAnalyticsStrategy<SumByCategoryInput, decimal>
{
    /// <summary>
    /// Состояние приложения с операциями.
    /// </summary>
    private readonly AppState _state;
    
    /// <summary>
    /// Конструктор стратегии.
    /// </summary>
    /// <param name="state">Состояние приложения</param>
    public SumByCategoryStrategy(AppState state) => _state = state;
    
    /// <summary>
    /// Выполняет подсчет суммы операций для выбранной категории.
    /// </summary>
    /// <param name="input">Входные данные с ID категории</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Task с результатом - суммой операций в категории</returns>
    public Task<decimal> CalculateAsync(SumByCategoryInput input, CancellationToken ct = default)
    {
        // LINQ:
        // 1. Where - фильтруем операции по категории
        // 2. Sum - суммируем SignedAmount
        var sum = _state.Operations
            .Where(o => o.CategoryId == input.CategoryId)  // Только операции этой категории
            .Sum(o => o.SignedAmount);  // Суммируем
        
        return Task.FromResult(sum);
    }
}