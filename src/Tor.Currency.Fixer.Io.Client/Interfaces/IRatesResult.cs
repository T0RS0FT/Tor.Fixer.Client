using Tor.Currency.Fixer.Io.Client.Models;

namespace Tor.Currency.Fixer.Io.Client.Interfaces
{
    public interface IRatesResult
    {
        public string BaseCurrencyCode { get; set; }

        public List<CurrencyRateResult> Rates { get; set; }
    }
}
