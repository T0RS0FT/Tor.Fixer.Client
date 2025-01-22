using Tor.Currency.Fixer.Io.Client.Interfaces;
using Tor.Currency.Fixer.Io.Client.Models;

namespace Tor.Currency.Fixer.Io.Client.Extensions
{
    public static class RatesExtensions
    {
        public static decimal Convert(
            this IRatesResult rates,
            string sourceCurrencyCode,
            string destinationCurrencyCode,
            decimal quantity)
        {
            ArgumentNullException.ThrowIfNull(rates);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(sourceCurrencyCode);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(destinationCurrencyCode);

            if (sourceCurrencyCode.Equals(destinationCurrencyCode, StringComparison.InvariantCultureIgnoreCase))
            {
                return quantity;
            }

            if (sourceCurrencyCode.Equals(rates.BaseCurrencyCode, StringComparison.InvariantCultureIgnoreCase))
            {
                var rate = rates.Rates.SingleOrDefault(x => x.CurrencyCode.Equals(destinationCurrencyCode, StringComparison.InvariantCultureIgnoreCase));

                return rate != null
                    ? rate.ExchangeRate * quantity
                    : throw new Exception("Destination currency code not found");
            }

            if (destinationCurrencyCode.Equals(rates.BaseCurrencyCode, StringComparison.InvariantCultureIgnoreCase))
            {
                var rate = rates.Rates.SingleOrDefault(x => x.CurrencyCode.Equals(sourceCurrencyCode, StringComparison.InvariantCultureIgnoreCase));

                return rate != null
                    ? 1 / rate.ExchangeRate * quantity
                    : throw new Exception("Source currency code not found");
            }

            var sourceRate = rates.Rates.SingleOrDefault(x => x.CurrencyCode.Equals(sourceCurrencyCode, StringComparison.InvariantCultureIgnoreCase));
            var destinationRate = rates.Rates.SingleOrDefault(x => x.CurrencyCode.Equals(destinationCurrencyCode, StringComparison.InvariantCultureIgnoreCase));

            return sourceRate == null
                ? throw new Exception("Source currency code not found")
                : destinationRate == null
                    ? throw new Exception("Destination currency code not found")
                    : destinationRate.ExchangeRate / sourceRate.ExchangeRate * quantity;
        }

        public static LatestRatesResult ChangeBaseCurrency(this LatestRatesResult rates, string baseCurrencyCode)
        {
            ArgumentNullException.ThrowIfNull(rates);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(baseCurrencyCode);

            return new LatestRatesResult()
            {
                Date = rates.Date,
                BaseCurrencyCode = baseCurrencyCode.ToUpper(),
                Timestamp = rates.Timestamp,
                Rates = RecalculateRates(rates, baseCurrencyCode)
            };
        }

        public static HistoricalRatesResult ChangeBaseCurrency(this HistoricalRatesResult rates, string baseCurrencyCode)
        {
            ArgumentNullException.ThrowIfNull(rates);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(baseCurrencyCode);

            return new HistoricalRatesResult()
            {
                Date = rates.Date,
                BaseCurrencyCode = baseCurrencyCode.ToUpper(),
                Timestamp = rates.Timestamp,
                Historical = rates.Historical,
                Rates = RecalculateRates(rates, baseCurrencyCode)
            };
        }

        private static List<CurrencyRateResult> RecalculateRates(IRatesResult rates, string baseCurrencyCode)
        {
            if (rates.BaseCurrencyCode.Equals(baseCurrencyCode, StringComparison.InvariantCultureIgnoreCase))
            {
                return rates.Rates.Select(x => new CurrencyRateResult()
                {
                    CurrencyCode = x.CurrencyCode,
                    ExchangeRate = x.ExchangeRate
                }).ToList();
            }

            var baseRate = rates.Rates.SingleOrDefault(x => x.CurrencyCode.Equals(baseCurrencyCode, StringComparison.InvariantCultureIgnoreCase))
                ?? throw new Exception("Currency code not found");

            return [.. rates.Rates
                .Select(x => x.CurrencyCode.Equals(baseCurrencyCode, StringComparison.InvariantCultureIgnoreCase)
                    ? new CurrencyRateResult()
                    {
                        CurrencyCode = rates.BaseCurrencyCode,
                        ExchangeRate = 1 / x.ExchangeRate
                    }
                    : new CurrencyRateResult()
                    {
                        CurrencyCode = x.CurrencyCode,
                        ExchangeRate = x.ExchangeRate / baseRate.ExchangeRate
                    })
                .OrderBy(x => x.CurrencyCode)];
        }
    }
}