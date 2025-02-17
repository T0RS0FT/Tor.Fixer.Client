using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using Tor.Fixer.Client.Enums;
using Tor.Fixer.Client.Extensions;
using Tor.Fixer.Client.Internal;
using Tor.Fixer.Client.Internal.Models;
using Tor.Fixer.Client.Models;

namespace Tor.Fixer.Client
{
    public class FixerClient(HttpClient httpClient, IOptions<FixerOptions> options) : IFixerClient
    {
        private readonly HttpClient httpClient = httpClient;
        private readonly FixerOptions options = options.Value;

        public async Task<bool> HealthCheckAsync()
        {
            var httpResponse = await httpClient.GetAsync(string.Empty);

            return httpResponse.IsSuccessStatusCode;
        }

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
                    destinationCurrencyCodes.ToFixerCurrencyCodes());
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
                    destinationCurrencyCodes.ToFixerCurrencyCodes());
            }

            return await GetFixerResponseAsync(
                date.ToFixerFormat(),
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
                queryParameters.Add(Constants.Endpoints.Convert.Parameters.Date, date.Value.ToFixerFormat());
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
                { Constants.Endpoints.TimeSeries.Parameters.StartDate, startDate.ToFixerFormat() },
                { Constants.Endpoints.TimeSeries.Parameters.EndDate, endDate.ToFixerFormat() },
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
                    destinationCurrencyCodes.ToFixerCurrencyCodes());
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
                { Constants.Endpoints.Fluctuation.Parameters.StartDate, startDate.ToFixerFormat() },
                { Constants.Endpoints.Fluctuation.Parameters.EndDate, endDate.ToFixerFormat() },
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
                    destinationCurrencyCodes.ToFixerCurrencyCodes());
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
            if (!string.IsNullOrWhiteSpace(options.BaseUrl))
            {
                httpClient.BaseAddress = new Uri(options.BaseUrl);
            }

            var httpResponse = await httpClient.GetAsync(GetUrl(url, queryParameters));

            switch (options.HttpErrorHandlingMode)
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
            var apiKey = options.ApiKeyFactory?.Invoke() ?? options.ApiKey;

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