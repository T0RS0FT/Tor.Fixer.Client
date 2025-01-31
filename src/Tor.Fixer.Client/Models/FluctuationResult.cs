namespace Tor.Fixer.Client.Models
{
    public class FluctuationResult
    {
        public bool Fluctuation { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public string BaseCurrencyCode { get; set; }

        public List<FluctuationRateResult> Rates { get; set; }
    }

    public class FluctuationRateResult
    {
        public string CurrencyCode { get; set; }

        public decimal StartRate { get; set; }

        public decimal EndRate { get; set; }

        public decimal Change { get; set; }

        public decimal ChangePercentage { get; set; }
    }
}
