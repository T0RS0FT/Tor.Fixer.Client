using Tor.Currency.Fixer.Io.Client.Interfaces;
using Tor.Currency.Fixer.Io.Client.Models;

namespace Tor.Currency.Fixer.Io.Client.Extensions
{
    public static class RatesExtensions
    {
        public static decimal Convert(
            this IRates rates,
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

        // TODO unit tests
        public static LatestRates ChangeBaseCurrency(this LatestRates rates, string baseCurrencyCode)
        {
            ArgumentNullException.ThrowIfNull(rates);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(baseCurrencyCode);

            return new LatestRates()
            {
                Date = rates.Date,
                BaseCurrencyCode = baseCurrencyCode.ToUpper(),
                Timestamp = rates.Timestamp,
                Rates = RecalculateRates(rates, baseCurrencyCode)
            };
        }

        // TODO unit tests
        public static HistoricalRates ChangeBaseCurrency(this HistoricalRates rates, string baseCurrencyCode)
        {
            ArgumentNullException.ThrowIfNull(rates);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(baseCurrencyCode);

            return new HistoricalRates()
            {
                Date = rates.Date,
                BaseCurrencyCode = baseCurrencyCode.ToUpper(),
                Timestamp = rates.Timestamp,
                Historical = rates.Historical,
                Rates = RecalculateRates(rates, baseCurrencyCode)
            };
        }

        private static List<CurrencyRate> RecalculateRates(IRates rates, string baseCurrencyCode)
        {
            if (rates.BaseCurrencyCode.Equals(baseCurrencyCode, StringComparison.InvariantCultureIgnoreCase))
            {
                return rates.Rates.Select(x => new CurrencyRate()
                {
                    CurrencyCode = x.CurrencyCode,
                    ExchangeRate = x.ExchangeRate
                }).ToList();
            }

            var baseRate = rates.Rates.SingleOrDefault(x => x.CurrencyCode.Equals(baseCurrencyCode, StringComparison.InvariantCultureIgnoreCase))
                ?? throw new Exception("Currency code not found");

            return [.. rates.Rates
                .Select(x => x.CurrencyCode.Equals(baseCurrencyCode, StringComparison.InvariantCultureIgnoreCase)
                    ? new CurrencyRate()
                    {
                        CurrencyCode = rates.BaseCurrencyCode,
                        ExchangeRate = 1 / x.ExchangeRate
                    }
                    : new CurrencyRate()
                    {
                        CurrencyCode = x.CurrencyCode,
                        ExchangeRate = baseRate.ExchangeRate / x.ExchangeRate
                    })
                .OrderBy(x => x.CurrencyCode)];
        }
    }
}