using System.Text.Json.Serialization;
using Tor.Fixer.Client.Enums;
using Tor.Fixer.Client.Models;

namespace Tor.Fixer.Client.Internal.Models
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
                Code = Code,
                Type = Type,
                Info = Info,
            };
        }
    }
}