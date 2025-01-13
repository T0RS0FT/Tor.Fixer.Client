namespace Tor.Currency.Fixer.Io.Client.Models
{
    public class FixerResponse<T>
    {
        public bool Success { get; set; }

        public T Data { get; set; }

        public FixerError Error { get; set; }
    }
}
