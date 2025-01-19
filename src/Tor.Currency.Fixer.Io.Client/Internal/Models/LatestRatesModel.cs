using System.Text.Json.Serialization;

namespace Tor.Currency.Fixer.Io.Client.Internal.Models
{
    internal class LatestRatesModel : FixerModelBase
    {
        [JsonInclude]
        internal int TimeStamp { get; set; }

        [JsonInclude]
        internal string Base { get; set; }

        [JsonInclude]
        internal DateTime Date { get; set; }

        [JsonInclude]
        internal Dictionary<string, decimal> Rates { get; set; }
    }
}
