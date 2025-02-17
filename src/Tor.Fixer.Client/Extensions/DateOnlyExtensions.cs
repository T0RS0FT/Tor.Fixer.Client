namespace Tor.Fixer.Client.Extensions
{
    internal static class DateOnlyExtensions
    {
        internal static string ToFixerFormat(this DateOnly date)
            => date.ToString("O");
    }
}
