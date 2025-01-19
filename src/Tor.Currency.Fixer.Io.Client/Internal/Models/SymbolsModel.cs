using System.Text.Json.Serialization;

namespace Tor.Currency.Fixer.Io.Client.Internal.Models
{
    internal class SymbolsModel : FixerModelBase
    {
        [JsonInclude]
        internal Dictionary<string, string> Symbols { get; set; }
    }
}