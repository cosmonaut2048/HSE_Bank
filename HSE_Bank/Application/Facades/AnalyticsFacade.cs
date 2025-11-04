namespace HSE_Bank.Application.Facades;

// Реализует паттерн Facade

/// <summary>
/// Фасад для выполнения аналитики и анализа финансовых данных.
/// </summary>
public sealed class AnalyticsFacade
{
    /// <summary>
    /// Стратегия анализа сумм за выбранный период времени.
    /// </summary>
    private readonly Application.Analytics.SumByPeriodStrategy _byPeriod;
    
    /// <summary>
    /// Стратегия анализа сумм по конкретной категории.
    /// </summary>
    private readonly Application.Analytics.SumByCategoryStrategy _byCategory;
    
    /// <summary>
    /// Стратегия проверки аномалий в операциях.
    /// </summary>
    private readonly Application.Analytics.AnomalyCheckStrategy _anomaly;
    
    /// <summary>
    /// Конструктор фасада аналитики.
    /// </summary>
    /// <param name="byPeriod">Стратегия анализа по периодам</param>
    /// <param name="byCategory">Стратегия анализа по категориям</param>
    /// <param name="anomaly">Стратегия проверки аномалий</param>
    public AnalyticsFacade(Application.Analytics.SumByPeriodStrategy byPeriod, Application.Analytics.SumByCategoryStrategy byCategory, Application.Analytics.AnomalyCheckStrategy anomaly)
    {
        _byPeriod = byPeriod;
        _byCategory = byCategory;
        _anomaly = anomaly;
    }
    
    /// <summary>
    /// Асинхронно подсчитывает сумму всех операций за выбранный период.
    /// </summary>
    /// <param name="from">Начальная дата диапазона (включительно)</param>
    /// <param name="to">Конечная дата диапазона (включительно)</param>
    /// <param name="ct">Токен отмены для прерывания операции</param>
    /// <returns>Task содержащий итоговую сумму за период</returns>
    public Task<decimal> SumByPeriodAsync(DateTime from, DateTime to, CancellationToken ct = default)
        => _byPeriod.CalculateAsync(new Application.Analytics.SumByPeriodInput(from, to), ct);
    
    /// <summary>
    /// Асинхронно подсчитывает сумму всех операций по конкретной категории.
    /// </summary>
    /// <param name="categoryId">ID категории для анализа</param>
    /// <param name="ct">Токен отмены для прерывания операции</param>
    /// <returns>Task содержащий итоговую сумму по категории</returns>
    public Task<decimal> SumByCategoryAsync(Guid categoryId, CancellationToken ct = default)
        => _byCategory.CalculateAsync(new Application.Analytics.SumByCategoryInput(categoryId), ct);
    
    /// <summary>
    /// Асинхронно проверяет наличие аномалий в операциях.
    /// </summary>
    /// <param name="threshold">Пороговое значение для определения аномалии</param>
    /// <param name="ct">Токен отмены для прерывания операции</param>
    /// <returns>Task с результатом: true если обнаружены аномалии, false если нет</returns>
    public Task<bool> HasAnomaliesAsync(decimal threshold, CancellationToken ct = default)
        => _anomaly.CalculateAsync(new Application.Analytics.AnomalyCheckInput(threshold), ct);
}