namespace Tor.Fixer.Client.Models
{
    public class TimeSeriesResult
    {
        public bool TimeSeries { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public string BaseCurrencyCode { get; set; }

        public List<TimeSeriesItemResult> Items { get; set; }
    }

    public class TimeSeriesItemResult
    {
        public DateOnly Date { get; set; }

        public List<CurrencyRateResult> Rates { get; set; }
    }
}
