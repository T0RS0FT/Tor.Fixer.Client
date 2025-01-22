namespace Tor.Fixer.Client.Models
{
    public class FixerResponse<TResult>
    {
        public bool Success { get; set; }

        public TResult Result { get; set; }

        public FixerError Error { get; set; }
    }
}
