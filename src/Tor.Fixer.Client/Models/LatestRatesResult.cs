using Tor.Fixer.Client.Interfaces;

namespace Tor.Fixer.Client.Models
{
    public class LatestRatesResult : IRatesResult
    {
        public string BaseCurrencyCode { get; set; }

        public DateOnly Date { get; set; }

        public long Timestamp { get; set; }

        public List<CurrencyRateResult> Rates { get; set; }
    }
}