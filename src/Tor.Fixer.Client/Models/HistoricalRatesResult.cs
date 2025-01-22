using Tor.Fixer.Client.Interfaces;

namespace Tor.Fixer.Client.Models
{
    public class HistoricalRatesResult : IRatesResult
    {
        public bool Historical { get; set; }

        public string BaseCurrencyCode { get; set; }

        public DateTime Date { get; set; }

        public int Timestamp { get; set; }

        public List<CurrencyRateResult> Rates { get; set; }
    }
}