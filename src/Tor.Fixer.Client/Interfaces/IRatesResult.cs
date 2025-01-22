using Tor.Fixer.Client.Models;

namespace Tor.Fixer.Client.Interfaces
{
    public interface IRatesResult
    {
        public string BaseCurrencyCode { get; set; }

        public List<CurrencyRateResult> Rates { get; set; }
    }
}
