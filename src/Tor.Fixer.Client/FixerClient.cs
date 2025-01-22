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
            => await GetFixerResponseAsync("symbols", [], Mappers.Symbols);

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
                queryParameters.Add("base", baseCurrencyCode);
            }

            if (destinationCurrencyCodes != null && destinationCurrencyCodes.Length > 0)
            {
                queryParameters.Add("symbols", string.Join(",", destinationCurrencyCodes));
            }

            return await GetFixerResponseAsync("latest", queryParameters, Mappers.LatestRates);
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
                queryParameters.Add("base", baseCurrencyCode);
            }

            if (destinationCurrencyCodes != null && destinationCurrencyCodes.Length > 0)
            {
                queryParameters.Add("symbols", string.Join(",", destinationCurrencyCodes));
            }

            return await GetFixerResponseAsync(
                $"{date.Year:0000}-{date.Month:00}-{date.Day:00}",
                queryParameters,
                Mappers.HistoricalRates);
        }

        public async Task<FixerResponse<ConvertResult>> ConvertAsync(string sourceCurrencyCode, string destinationCurrencyCode, decimal amount)
            => await ConvertAsync(sourceCurrencyCode, destinationCurrencyCode, amount, null);

        public async Task<FixerResponse<ConvertResult>> ConvertAsync(string sourceCurrencyCode, string destinationCurrencyCode, decimal amount, DateTime? date)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(sourceCurrencyCode);
            ArgumentException.ThrowIfNullOrWhiteSpace(destinationCurrencyCode);

            var queryParameters = new Dictionary<string, string>
            {
                { "from", sourceCurrencyCode },
                { "to", destinationCurrencyCode },
                { "amount", amount.ToString("#.#") }
            };

            if (date != null)
            {
                queryParameters.Add("date", $"{date.Value.Year:0000}-{date.Value.Month:00}-{date.Value.Day:00}");
            }

            return await GetFixerResponseAsync(
                "convert",
                queryParameters,
                Mappers.Convert);
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

            var content = await httpResponse.Content.ReadFromJsonAsync<TFixerModel>();

            Console.WriteLine(content.Error);

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