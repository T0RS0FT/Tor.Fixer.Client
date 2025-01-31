using Tor.Fixer.Client.Models;

namespace Tor.Fixer.Client
{
    public interface IFixerClient
    {
        Task<FixerResponse<List<SymbolResult>>> GetSymbolsAsync();

        Task<FixerResponse<LatestRatesResult>> GetLatestRatesAsync();

        Task<FixerResponse<LatestRatesResult>> GetLatestRatesAsync(string baseCurrencyCode);

        Task<FixerResponse<LatestRatesResult>> GetLatestRatesAsync(string[] destinationCurrencyCodes);

        Task<FixerResponse<LatestRatesResult>> GetLatestRatesAsync(string baseCurrencyCode, string[] destinationCurrencyCodes);

        Task<FixerResponse<HistoricalRatesResult>> GetHistoricalRatesAsync(DateOnly date);

        Task<FixerResponse<HistoricalRatesResult>> GetHistoricalRatesAsync(DateOnly date, string baseCurrencyCode);

        Task<FixerResponse<HistoricalRatesResult>> GetHistoricalRatesAsync(DateOnly date, string[] destinationCurrencyCodes);

        Task<FixerResponse<HistoricalRatesResult>> GetHistoricalRatesAsync(DateOnly date, string baseCurrencyCode, string[] destinationCurrencyCodes);

        Task<FixerResponse<ConvertResult>> ConvertAsync(string sourceCurrencyCode, string destinationCurrencyCode, decimal amount);

        Task<FixerResponse<ConvertResult>> ConvertAsync(string sourceCurrencyCode, string destinationCurrencyCode, decimal amount, DateOnly? date);

        Task<FixerResponse<TimeSeriesResult>> GetTimeSeriesAsync(DateOnly startDate, DateOnly endDate);

        Task<FixerResponse<TimeSeriesResult>> GetTimeSeriesAsync(DateOnly startDate, DateOnly endDate, string baseCurrencyCode);

        Task<FixerResponse<TimeSeriesResult>> GetTimeSeriesAsync(DateOnly startDate, DateOnly endDate, string[] destinationCurrencyCodes);

        Task<FixerResponse<TimeSeriesResult>> GetTimeSeriesAsync(DateOnly startDate, DateOnly endDate, string baseCurrencyCode, string[] destinationCurrencyCodes);
    }
}
