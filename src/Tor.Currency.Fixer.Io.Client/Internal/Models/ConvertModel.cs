using System.Text.Json.Serialization;

namespace Tor.Currency.Fixer.Io.Client.Internal.Models
{
    internal class ConvertModel : FixerModelBase
    {
        [JsonInclude]
        internal bool Historical { get; set; }

        [JsonInclude]
        internal DateTime Date { get; set; }

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
        public int Timestamp { get; set; }

        [JsonInclude]
        public decimal Rate { get; set; }
    }
}
