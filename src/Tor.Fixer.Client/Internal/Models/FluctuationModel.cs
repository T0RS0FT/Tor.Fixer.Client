using System.Text.Json.Serialization;
using Tor.Fixer.Client.Json;

namespace Tor.Fixer.Client.Internal.Models
{
    internal class FluctuationModel : FixerModelBase
    {
        [JsonInclude]
        [JsonConverter(typeof(SafeBoolConverter))]
        internal bool Fluctuation { get; set; }

        [JsonInclude]
        internal DateOnly StartDate { get; set; }

        [JsonInclude]
        internal DateOnly EndDate { get; set; }

        [JsonInclude]
        internal string Base { get; set; }

        [JsonInclude]
        internal Dictionary<string, FluctuationRateModel> Rates { get; set; }
    }

    internal class FluctuationRateModel
    {
        [JsonInclude]
        internal decimal StartRate { get; set; }

        [JsonInclude]
        internal decimal EndRate { get; set; }

        [JsonInclude]
        internal decimal Change { get; set; }

        [JsonInclude]
        internal decimal ChangePct { get; set; }
    }
}
