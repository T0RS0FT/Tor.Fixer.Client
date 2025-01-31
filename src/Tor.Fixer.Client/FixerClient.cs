using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using Tor.Fixer.Client.Enums;
using Tor.Fixer.Client.Internal;
using Tor.Fixer.Client.Internal.Models;
using Tor.Fixer.Client.Models;

namespace Tor.Fixer.Client
{
    public class FixerClient(HttpClient httpClient, IOptions<FixerOptions> options) : IFixerClient
    {
        public async Task<FixerResponse<List<SymbolResult>>> GetSymbolsAsync()
            => await GetFixerResponseAsync(Constants.Endpoints.Symbols.UrlSegment, [], Mappers.Symbols);

        public async Task<FixerResponse<LatestRatesResult>> GetLatestRatesAsync()
            => await GetLatestRatesAsync(null, null);

        public async Task<FixerResponse<LatestRatesResult>> GetLatestRatesAsync(string baseCurrencyCode)
            => await GetLatestRatesAsync(baseCurrencyCode, null);

        public async Task<FixerResponse<LatestRatesResult>> GetLatestRatesAsync(string[] destinationCurrencyCodes)
            => await GetLatestRatesAsync(null, destinationCurrencyCodes);

        public async Task<FixerResponse<LatestRatesResult>> GetLatestRatesAsync(string baseCurrencyCode, string[] destinationCurrencyCodes)
        {
            var queryParameters = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(baseCurrencyCode))
            {
                queryParameters.Add(
                    Constants.Endpoints.LatestRates.Parameters.BaseCurrencyCode,
                    baseCurrencyCode);
            }

            if (destinationCurrencyCodes != null && destinationCurrencyCodes.Length > 0)
            {
                queryParameters.Add(
                    Constants.Endpoints.LatestRates.Parameters.Symbols,
                    string.Join(",", destinationCurrencyCodes));
            }

            return await GetFixerResponseAsync(
                Constants.Endpoints.LatestRates.UrlSegment,
                queryParameters,
                Mappers.LatestRates);
        }

        public async Task<FixerResponse<HistoricalRatesResult>> GetHistoricalRatesAsync(DateOnly date)
            => await GetHistoricalRatesAsync(date, null, null);

        public async Task<FixerResponse<HistoricalRatesResult>> GetHistoricalRatesAsync(DateOnly date, string baseCurrencyCode)
            => await GetHistoricalRatesAsync(date, baseCurrencyCode, null);

        public async Task<FixerResponse<HistoricalRatesResult>> GetHistoricalRatesAsync(DateOnly date, string[] destinationCurrencyCodes)
            => await GetHistoricalRatesAsync(date, null, destinationCurrencyCodes);

        public async Task<FixerResponse<HistoricalRatesResult>> GetHistoricalRatesAsync(DateOnly date, string baseCurrencyCode, string[] destinationCurrencyCodes)
        {
            var queryParameters = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(baseCurrencyCode))
            {
                queryParameters.Add(
                    Constants.Endpoints.HistoricalRates.Parameters.BaseCurrencyCode,
                    baseCurrencyCode);
            }

            if (destinationCurrencyCodes != null && destinationCurrencyCodes.Length > 0)
            {
                queryParameters.Add(
                    Constants.Endpoints.HistoricalRates.Parameters.Symbols,
                    string.Join(",", destinationCurrencyCodes));
            }

            return await GetFixerResponseAsync(
                date.ToString("O"),
                queryParameters,
                Mappers.HistoricalRates);
        }

        public async Task<FixerResponse<ConvertResult>> ConvertAsync(string sourceCurrencyCode, string destinationCurrencyCode, decimal amount)
            => await ConvertAsync(sourceCurrencyCode, destinationCurrencyCode, amount, null);

        public async Task<FixerResponse<ConvertResult>> ConvertAsync(string sourceCurrencyCode, string destinationCurrencyCode, decimal amount, DateOnly? date)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(sourceCurrencyCode);
            ArgumentException.ThrowIfNullOrWhiteSpace(destinationCurrencyCode);

            var queryParameters = new Dictionary<string, string>
            {
                { Constants.Endpoints.Convert.Parameters.SourceCurrencyCode, sourceCurrencyCode },
                { Constants.Endpoints.Convert.Parameters.DestinationCurrencyCode, destinationCurrencyCode },
                { Constants.Endpoints.Convert.Parameters.Amount, amount.ToString("#.#") }
            };

            if (date != null)
            {
                queryParameters.Add(Constants.Endpoints.Convert.Parameters.Date, date.Value.ToString("O"));
            }

            return await GetFixerResponseAsync(
                Constants.Endpoints.Convert.UrlSegment,
                queryParameters,
                Mappers.Convert);
        }

        public async Task<FixerResponse<TimeSeriesResult>> GetTimeSeriesAsync(DateOnly startDate, DateOnly endDate)
            => await GetTimeSeriesAsync(startDate, endDate, null, null);

        public async Task<FixerResponse<TimeSeriesResult>> GetTimeSeriesAsync(DateOnly startDate, DateOnly endDate, string baseCurrencyCode)
            => await GetTimeSeriesAsync(startDate, endDate, baseCurrencyCode, null);

        public async Task<FixerResponse<TimeSeriesResult>> GetTimeSeriesAsync(DateOnly startDate, DateOnly endDate, string[] destinationCurrencyCodes)
            => await GetTimeSeriesAsync(startDate, endDate, null, destinationCurrencyCodes);

        public async Task<FixerResponse<TimeSeriesResult>> GetTimeSeriesAsync(DateOnly startDate, DateOnly endDate, string baseCurrencyCode, string[] destinationCurrencyCodes)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Constants.Endpoints.TimeSeries.Parameters.StartDate, startDate.ToString("O") },
                { Constants.Endpoints.TimeSeries.Parameters.EndDate, endDate.ToString("O") },
            };

            if (!string.IsNullOrWhiteSpace(baseCurrencyCode))
            {
                queryParameters.Add(
                    Constants.Endpoints.TimeSeries.Parameters.BaseCurrencyCode,
                    baseCurrencyCode);
            }

            if (destinationCurrencyCodes != null && destinationCurrencyCodes.Length > 0)
            {
                queryParameters.Add(
                    Constants.Endpoints.TimeSeries.Parameters.Symbols,
                    string.Join(",", destinationCurrencyCodes));
            }

            return await GetFixerResponseAsync(
                Constants.Endpoints.TimeSeries.UrlSegment,
                queryParameters,
                Mappers.TimeSeries);
        }

        public async Task<FixerResponse<FluctuationResult>> GetFluctuationAsync(DateOnly startDate, DateOnly endDate)
            => await GetFluctuationAsync(startDate, endDate, null, null);

        public async Task<FixerResponse<FluctuationResult>> GetFluctuationAsync(DateOnly startDate, DateOnly endDate, string baseCurrencyCode)
            => await GetFluctuationAsync(startDate, endDate, baseCurrencyCode, null);

        public async Task<FixerResponse<FluctuationResult>> GetFluctuationAsync(DateOnly startDate, DateOnly endDate, string[] destinationCurrencyCodes)
            => await GetFluctuationAsync(startDate, endDate, null, destinationCurrencyCodes);

        public async Task<FixerResponse<FluctuationResult>> GetFluctuationAsync(DateOnly startDate, DateOnly endDate, string baseCurrencyCode, string[] destinationCurrencyCodes)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Constants.Endpoints.Fluctuation.Parameters.StartDate, startDate.ToString("O") },
                { Constants.Endpoints.Fluctuation.Parameters.EndDate, endDate.ToString("O") },
            };

            if (!string.IsNullOrWhiteSpace(baseCurrencyCode))
            {
                queryParameters.Add(
                    Constants.Endpoints.Fluctuation.Parameters.BaseCurrencyCode,
                    baseCurrencyCode);
            }

            if (destinationCurrencyCodes != null && destinationCurrencyCodes.Length > 0)
            {
                queryParameters.Add(
                    Constants.Endpoints.Fluctuation.Parameters.Symbols,
                    string.Join(",", destinationCurrencyCodes));
            }

            return await GetFixerResponseAsync(
                Constants.Endpoints.Fluctuation.UrlSegment,
                queryParameters,
                Mappers.Fluctuation);
        }

        private async Task<FixerResponse<TResponseModel>> GetFixerResponseAsync<TFixerModel, TResponseModel>(
            string url,
            Dictionary<string, string> queryParameters,
            Func<TFixerModel, TResponseModel> mapper)
            where TFixerModel : FixerModelBase
        {
            if (!string.IsNullOrWhiteSpace(options.Value.BaseUrl))
            {
                httpClient.BaseAddress = new Uri(options.Value.BaseUrl);
            }

            var httpResponse = await httpClient.GetAsync(GetUrl(url, queryParameters));

            switch (options.Value.HttpErrorHandlingMode)
            {
                case HttpErrorHandlingMode.ThrowsException:
                    httpResponse.EnsureSuccessStatusCode();
                    break;
                case HttpErrorHandlingMode.ReturnsError:
                    if (!httpResponse.IsSuccessStatusCode)
                    {
                        return new FixerResponse<TResponseModel>()
                        {
                            Success = false,
                            Error = new FixerError()
                            {
                                ErrorType = ErrorType.Http,
                                Code = (int)httpResponse.StatusCode
                            }
                        };
                    }
                    break;
            }

            var content = await httpResponse.Content.ReadFromJsonAsync<TFixerModel>(Constants.JsonSerializerOptions);

            return new FixerResponse<TResponseModel>()
            {
                Success = content.Success,
                Error = content.Error?.ToFixerError(),
                Result = content.Success ? mapper.Invoke(content) : default
            };
        }

        private string GetApiKey()
        {
            var apiKey = options.Value.ApiKeyFactory?.Invoke() ?? options.Value.ApiKey;

            return !string.IsNullOrWhiteSpace(apiKey)
                ? apiKey
                : throw new Exception("API key required");
        }

        private string GetUrl(string url, Dictionary<string, string> queryParameters)
        {
            queryParameters ??= [];
            if (!queryParameters.ContainsKey(Constants.ApiKeyQueryParamName))
            {
                queryParameters.Add(Constants.ApiKeyQueryParamName, GetApiKey());
            }

            return $"{url}?{string.Join("&", queryParameters.Select(x => $"{x.Key}={x.Value}"))}";
        }
    }
}