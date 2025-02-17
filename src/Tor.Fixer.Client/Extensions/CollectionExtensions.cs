namespace Tor.Fixer.Client.Extensions
{
    internal static class CollectionExtensions
    {
        internal static string ToFixerCurrencyCodes(this string[] currencyCodes)
            => currencyCodes == null || currencyCodes.Length == 0 ? string.Empty : string.Join(",", currencyCodes);
    }
}
