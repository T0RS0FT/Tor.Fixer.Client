using Tor.Fixer.Client.Internal.Models;
using Tor.Fixer.Client.Models;

namespace Tor.Fixer.Client.Internal
{
    internal class Mappers
    {
        internal static readonly Func<SymbolsModel, List<SymbolResult>> Symbols = x =>
            x.Symbols?.Select(x => new SymbolResult()
            {
                Code = x.Key,
                Name = x.Value
            }).ToList() ?? [];

        internal static readonly Func<LatestRatesModel, LatestRatesResult> LatestRates = x =>
            new LatestRatesResult()
            {
                BaseCurrencyCode = x.Base,
                Date = x.Date,
                Timestamp = x.Timestamp,
                Rates = x.Rates?.Select(rate => new CurrencyRateResult()
                {
                    CurrencyCode = rate.Key,
                    ExchangeRate = rate.Value
                }).ToList() ?? []
            };

        internal static readonly Func<HistoricalRatesModel, HistoricalRatesResult> HistoricalRates = x =>
            new HistoricalRatesResult()
            {
                Historical = x.Historical,
                BaseCurrencyCode = x.Base,
                Date = x.Date,
                Timestamp = x.Timestamp,
                Rates = x.Rates?.Select(rate => new CurrencyRateResult()
                {
                    CurrencyCode = rate.Key,
                    ExchangeRate = rate.Value
                }).ToList() ?? []
            };

        internal static readonly Func<ConvertModel, ConvertResult> Convert = x =>
            new ConvertResult()
            {
                Historical = x.Historical,
                Date = x.Date,
                Result = x.Result,
                Query = x.Query == null
                    ? null
                    : new ConvertQueryResult()
                    {
                        SourceCurrencyCode = x.Query.From,
                        DestinationCurrencyCode = x.Query.To,
                        Amount = x.Query.Amount
                    },
                Info = x.Info == null
                    ? null
                    : new ConvertInfoResult()
                    {
                        Timestamp = x.Info.Timestamp,
                        Rate = x.Info.Rate
                    }
            };
    }
}