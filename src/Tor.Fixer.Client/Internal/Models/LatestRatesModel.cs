using System.Text.Json.Serialization;

namespace Tor.Fixer.Client.Internal.Models
{
    internal class LatestRatesModel : FixerModelBase
    {
        [JsonInclude]
        internal long Timestamp { get; set; }

        [JsonInclude]
        internal string Base { get; set; }

        [JsonInclude]
        internal DateOnly Date { get; set; }

        [JsonInclude]
        internal Dictionary<string, decimal> Rates { get; set; }
    }
}
