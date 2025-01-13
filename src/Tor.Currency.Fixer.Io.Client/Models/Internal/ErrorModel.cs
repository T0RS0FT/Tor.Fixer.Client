using Tor.Currency.Fixer.Io.Client.Enums;

namespace Tor.Currency.Fixer.Io.Client.Models.Internal
{
    internal class ErrorModel
    {
        public int Code { get; set; }

        public string Type { get; set; }

        public string Info { get; set; }

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