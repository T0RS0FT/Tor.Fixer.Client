using System.Text.Json.Serialization;
using Tor.Fixer.Client.Json;

namespace Tor.Fixer.Client.Internal.Models
{
    internal class TimeSeriesModel : FixerModelBase
    {
        [JsonInclude]
        [JsonConverter(typeof(SafeBoolConverter))]
        internal bool Timeseries { get; set; }

        [JsonInclude]
        internal DateOnly StartDate { get; set; }

        [JsonInclude]
        internal DateOnly EndDate { get; set; }

        [JsonInclude]
        internal string Base { get; set; }

        [JsonInclude]
        internal Dictionary<DateOnly, Dictionary<string, decimal>> Rates { get; set; }
    }
}