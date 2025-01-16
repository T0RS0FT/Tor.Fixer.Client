namespace Tor.Currency.Fixer.Io.Client.Models
{
    public class LatestRates
    {
        public string BaseCurrencyCode { get; set; }

        public DateTime Date { get; set; }

        public int Timestamp { get; set; }

        public List<CurrencyRate> Rates { get; set; }
    }
}