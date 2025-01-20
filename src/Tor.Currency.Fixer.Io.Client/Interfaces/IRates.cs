using Tor.Currency.Fixer.Io.Client.Models;

namespace Tor.Currency.Fixer.Io.Client.Interfaces
{
    public interface IRates
    {
        public string BaseCurrencyCode { get; set; }

        public List<CurrencyRate> Rates { get; set; }
    }
}
