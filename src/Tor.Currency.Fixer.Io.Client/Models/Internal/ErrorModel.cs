using System.Text.Json.Serialization;
using Tor.Currency.Fixer.Io.Client.Enums;

namespace Tor.Currency.Fixer.Io.Client.Models.Internal
{
    internal class ErrorModel
    {
        [JsonInclude]
        internal int Code { get; set; }

        [JsonInclude]
        internal string Type { get; set; }

        [JsonInclude]
        internal string Info { get; set; }

        public FixerError ToFixerError()
        {
            return new FixerError()
            {
                ErrorType = ErrorType.Fixer,
                Code = this.Code,
                Type = this.Type,
                Info = this.Info,
            };
        }
    }
}