using Tor.Currency.Fixer.Io.Client.Internal.Models;
using Tor.Currency.Fixer.Io.Client.Models;

namespace Tor.Currency.Fixer.Io.Client.Internal
{
    internal class Mappers
    {
        internal static readonly Func<SymbolsModel, List<Symbol>> Symbols = x =>
            x.Symbols?.Select(x => new Symbol()
            {
                Code = x.Key,
                Name = x.Value
            }).ToList() ?? [];

        internal static readonly Func<LatestRatesModel, LatestRates> LatestRates = x =>
            new LatestRates()
            {
                BaseCurrencyCode = x.Base,
                Date = x.Date,
                Timestamp = x.Timestamp,
                Rates = x.Rates?.Select(rate => new CurrencyRate()
                {
                    CurrencyCode = rate.Key,
                    ExchangeRate = rate.Value
                }).ToList() ?? []
            };

        internal static readonly Func<HistoricalRatesModel, HistoricalRates> HistoricalRates = x =>
            new HistoricalRates()
            {
                Historical = x.Historical,
                BaseCurrencyCode = x.Base,
                Date = x.Date,
                Timestamp = x.Timestamp,
                Rates = x.Rates?.Select(rate => new CurrencyRate()
                {
                    CurrencyCode = rate.Key,
                    ExchangeRate = rate.Value
                }).ToList() ?? []
            };
    }
}