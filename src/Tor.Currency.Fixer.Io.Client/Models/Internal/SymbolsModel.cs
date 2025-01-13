namespace Tor.Currency.Fixer.Io.Client.Models.Internal
{
    internal class SymbolsModel
    {
        internal bool Success { get; set; }

        internal ErrorModel Error { get; set; }

        internal Dictionary<string, string> Symbols { get; set; }
    }
}