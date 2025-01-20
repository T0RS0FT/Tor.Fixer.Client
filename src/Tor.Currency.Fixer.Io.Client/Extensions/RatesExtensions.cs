using Tor.Currency.Fixer.Io.Client.Interfaces;

namespace Tor.Currency.Fixer.Io.Client.Extensions
{
    // TODO unit tests
    public static class RatesExtensions
    {
        public static decimal Convert(
            this IRates rates,
            string sourceCurrencyCode,
            string destinationCurrencyCode,
            decimal quantity)
        {
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
    }
}
