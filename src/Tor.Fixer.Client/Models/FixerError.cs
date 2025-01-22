using Tor.Fixer.Client.Enums;

namespace Tor.Fixer.Client.Models
{
    public class FixerError
    {
        public ErrorType ErrorType { get; set; }

        public int Code { get; set; }

        public string Type { get; set; }

        public string Info { get; set; }
    }
}