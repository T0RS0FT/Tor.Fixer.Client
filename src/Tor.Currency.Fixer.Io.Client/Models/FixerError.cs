using Tor.Currency.Fixer.Io.Client.Enums;

namespace Tor.Currency.Fixer.Io.Client.Models
{
    public class FixerError
    {
        public ErrorType ErrorType { get; set; }

        public int Code { get; set; }

        public string Type { get; set; }

        public string Info { get; set; }
    }
}