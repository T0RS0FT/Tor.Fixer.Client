using System.Text.Json.Serialization;

namespace Tor.Currency.Fixer.Io.Client.Models.Internal
{
    internal class FixerModelBase
    {
        [JsonInclude]
        internal bool Success { get; set; }

        [JsonInclude]
        internal ErrorModel Error { get; set; }
    }
}
