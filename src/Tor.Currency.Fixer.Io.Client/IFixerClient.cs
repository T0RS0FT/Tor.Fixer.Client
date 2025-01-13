using Tor.Currency.Fixer.Io.Client.Models;

namespace Tor.Currency.Fixer.Io.Client
{
    public interface IFixerClient
    {
        Task<FixerResponse<List<Symbol>>> GetSymbolsAsync();
    }
}
