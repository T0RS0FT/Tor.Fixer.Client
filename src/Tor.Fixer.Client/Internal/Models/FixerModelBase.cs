using System.Text.Json.Serialization;

namespace Tor.Fixer.Client.Internal.Models
{
    internal class FixerModelBase
    {
        [JsonInclude]
        internal bool Success { get; set; }

        [JsonInclude]
        internal ErrorModel Error { get; set; }
    }
}
