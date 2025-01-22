using Tor.Fixer.Client.Interfaces;

namespace Tor.Fixer.Client.Models
{
    public class LatestRatesResult : IRatesResult
    {
        public string BaseCurrencyCode { get; set; }

        public DateTime Date { get; set; }

        public int Timestamp { get; set; }

        public List<CurrencyRateResult> Rates { get; set; }
    }
}