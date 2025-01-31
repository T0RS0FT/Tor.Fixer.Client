using System.Text.Json.Serialization;
using Tor.Fixer.Client.Json;

namespace Tor.Fixer.Client.Internal.Models
{
    internal class HistoricalRatesModel : FixerModelBase
    {
        [JsonInclude]
        [JsonConverter(typeof(SafeBoolConverter))]
        internal bool Historical { get; set; }

        [JsonInclude]
        internal int Timestamp { get; set; }

        [JsonInclude]
        internal string Base { get; set; }

        [JsonInclude]
        internal DateOnly Date { get; set; }

        [JsonInclude]
        internal Dictionary<string, decimal> Rates { get; set; }
    }
}
