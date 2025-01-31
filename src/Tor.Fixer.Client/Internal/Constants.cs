using System.Text.Json;

namespace Tor.Fixer.Client.Internal
{
    internal class Constants
    {
        internal const string DefaultFixerUrl = "https://data.fixer.io/api/";

        internal const string ApiKeyQueryParamName = "access_key";

        internal static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };
    }
}
