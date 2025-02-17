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

        internal class Endpoints
        {
            internal class Symbols
            {
                internal const string UrlSegment = "symbols";
            }

            internal class LatestRates
            {
                internal const string UrlSegment = "latest";

                internal class Parameters
                {
                    internal const string BaseCurrencyCode = "base";
                    internal const string Symbols = "symbols";
                }
            }

            internal class HistoricalRates
            {
                internal class Parameters
                {
                    internal const string BaseCurrencyCode = "base";
                    internal const string Symbols = "symbols";
                }
            }

            internal class Convert
            {
                internal const string UrlSegment = "convert";

                internal class Parameters
                {
                    internal const string SourceCurrencyCode = "from";
                    internal const string DestinationCurrencyCode = "to";
                    internal const string Amount = "amount";
                    internal const string Date = "date";
                }
            }

            internal class TimeSeries
            {
                internal const string UrlSegment = "timeseries";

                internal class Parameters
                {
                    internal const string StartDate = "start_date";
                    internal const string EndDate = "end_date";
                    internal const string BaseCurrencyCode = "base";
                    internal const string Symbols = "symbols";
                }
            }

            internal class Fluctuation
            {
                internal const string UrlSegment = "fluctuation";

                internal class Parameters
                {
                    internal const string StartDate = "start_date";
                    internal const string EndDate = "end_date";
                    internal const string BaseCurrencyCode = "base";
                    internal const string Symbols = "symbols";
                }
            }
        }

        internal class Messages
        {
            internal const string CurrencyCodeNotFound = "Currency code not found";
            internal const string SourceCurrencyCodeNotFound = "Source currency code not found";
            internal const string DestinationCurrencyCodeNotFound = "Destination currency code not found";
        }
    }
}
