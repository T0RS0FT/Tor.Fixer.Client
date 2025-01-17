using Tor.Currency.Fixer.Io.Client.Models;

namespace Tor.Currency.Fixer.Io.Client.BlazorDemo.Extensions
{
    public static class FixerErrorExtensions
    {
        public static string ToMessage(this FixerError error)
            => $"Code: {error.Code}, Type: '{error.Type}', Info: '{error.Info}'";
    }
}
