namespace HSE_Bank.Application.Analytics;

public interface IAnalyticsStrategy<TIn, TOut>
{
    Task<TOut> CalculateAsync(TIn input, CancellationToken ct = default);
}