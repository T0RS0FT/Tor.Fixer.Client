namespace Tor.Fixer.Client.Extensions
{
    internal static class StringExtensions
    {
        internal static bool IgnoreCaseEquals(this string str1, string str2)
            => str1.Equals(str2, StringComparison.InvariantCultureIgnoreCase);
    }
}
