using Tor.Currency.Fixer.Io.Client.Models;

namespace Tor.Currency.Fixer.Io.Client
{
    public interface IFixerClient
    {
        Task<FixerResponse<List<Symbol>>> GetSymbolsAsync();

        Task<FixerResponse<LatestRates>> GetLatestRatesAsync(string baseCurrencyCode, params string[] symbols);
    }
}
