using System.Text.Json.Serialization;
using Tor.Fixer.Client.Json;

namespace Tor.Fixer.Client.Internal.Models
{
    internal class ConvertModel : FixerModelBase
    {
        [JsonInclude]
        [JsonConverter(typeof(SafeBoolConverter))]
        internal bool Historical { get; set; }

        [JsonInclude]
        internal DateOnly Date { get; set; }

        [JsonInclude]
        internal decimal Result { get; set; }

        [JsonInclude]
        internal ConvertQueryModel Query { get; set; }

        [JsonInclude]
        internal ConvertInfoModel Info { get; set; }
    }

    internal class ConvertQueryModel
    {
        [JsonInclude]
        public string From { get; set; }

        [JsonInclude]
        public string To { get; set; }

        [JsonInclude]
        public decimal Amount { get; set; }
    }

    internal class ConvertInfoModel
    {
        [JsonInclude]
        public long Timestamp { get; set; }

        [JsonInclude]
        public decimal Rate { get; set; }
    }
}
