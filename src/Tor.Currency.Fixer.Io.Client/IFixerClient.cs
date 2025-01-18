using Tor.Currency.Fixer.Io.Client.Models;

namespace Tor.Currency.Fixer.Io.Client
{
    public interface IFixerClient
    {
        Task<FixerResponse<List<Symbol>>> GetSymbolsAsync();

        Task<FixerResponse<LatestRates>> GetLatestRatesAsync();

        Task<FixerResponse<LatestRates>> GetLatestRatesAsync(string baseCurrencyCode);

        Task<FixerResponse<LatestRates>> GetLatestRatesAsync(string[] destinationCurrencyCodes);

        Task<FixerResponse<LatestRates>> GetLatestRatesAsync(string baseCurrencyCode, string[] destinationCurrencyCodes);
    }
}
