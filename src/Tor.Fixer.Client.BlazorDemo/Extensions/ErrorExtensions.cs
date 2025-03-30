using Tor.Fixer.Client.Models;

namespace Tor.Fixer.Client.BlazorDemo.Extensions
{
    public static class ErrorExtensions
    {
        public static string ToMessage(this FixerError error)
            => $"Code: {error.Code}, Type: '{error.Type}', Info: '{error.Info}'";
    }
}
