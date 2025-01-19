using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using Tor.Currency.Fixer.Io.Client.Enums;
using Tor.Currency.Fixer.Io.Client.Internal;
using Tor.Currency.Fixer.Io.Client.Internal.Models;
using Tor.Currency.Fixer.Io.Client.Models;

namespace Tor.Currency.Fixer.Io.Client
{
    public class FixerClient(HttpClient httpClient, IOptions<FixerOptions> options) : IFixerClient
    {
        public async Task<FixerResponse<List<Symbol>>> GetSymbolsAsync()
            => await this.GetFixerResponseAsync("symbols", [], Mappers.Symbols);

        public async Task<FixerResponse<LatestRates>> GetLatestRatesAsync()
            => await this.GetLatestRatesAsync(null, null);

        public async Task<FixerResponse<LatestRates>> GetLatestRatesAsync(string baseCurrencyCode)
            => await this.GetLatestRatesAsync(baseCurrencyCode, null);

        public async Task<FixerResponse<LatestRates>> GetLatestRatesAsync(string[] destinationCurrencyCodes)
            => await this.GetLatestRatesAsync(null, destinationCurrencyCodes);

        public async Task<FixerResponse<LatestRates>> GetLatestRatesAsync(string baseCurrencyCode, string[] destinationCurrencyCodes)
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

            return await this.GetFixerResponseAsync("latest", queryParameters, Mappers.LatestRates);
        }

        public async Task<FixerResponse<HistoricalRates>> GetHistoricalRatesAsync(DateOnly date)
            => await this.GetHistoricalRatesAsync(date, null, null);

        public async Task<FixerResponse<HistoricalRates>> GetHistoricalRatesAsync(DateOnly date, string baseCurrencyCode)
            => await this.GetHistoricalRatesAsync(date, baseCurrencyCode, null);

        public async Task<FixerResponse<HistoricalRates>> GetHistoricalRatesAsync(DateOnly date, string[] destinationCurrencyCodes)
            => await this.GetHistoricalRatesAsync(date, null, destinationCurrencyCodes);

        public async Task<FixerResponse<HistoricalRates>> GetHistoricalRatesAsync(DateOnly date, string baseCurrencyCode, string[] destinationCurrencyCodes)
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

            return await this.GetFixerResponseAsync(
                $"{date.Year:0000}-{date.Month:00}-{date.Day:00}",
                queryParameters,
                Mappers.HistoricalRates);
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

            var httpResponse = await httpClient.GetAsync(this.GetUrl(url, queryParameters));

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
                Data = content.Success ? mapper.Invoke(content) : default
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
                queryParameters.Add(Constants.ApiKeyQueryParamName, this.GetApiKey());
            }

            return $"{url}?{string.Join("&", queryParameters.Select(x => $"{x.Key}={x.Value}"))}";
        }
    }
}