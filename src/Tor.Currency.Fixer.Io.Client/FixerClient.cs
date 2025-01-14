using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using Tor.Currency.Fixer.Io.Client.Enums;
using Tor.Currency.Fixer.Io.Client.Models;
using Tor.Currency.Fixer.Io.Client.Models.Internal;

namespace Tor.Currency.Fixer.Io.Client
{
    public class FixerClient(HttpClient httpClient, IOptions<FixerOptions> options) : IFixerClient
    {
        public async Task<FixerResponse<List<Symbol>>> GetSymbolsAsync()
        {
            return await this.GetFixerResponse<SymbolsModel, List<Symbol>>(
                "symbols",
                [],
                x => x.Symbols.Select(x => new Symbol() { Code = x.Key, Name = x.Value }).ToList());
        }

        private async Task<FixerResponse<TResponseModel>> GetFixerResponse<TFixerModel, TResponseModel>(
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

            return queryParameters == null || queryParameters.Count == 0
                ? url
                : $"{url}?{string.Join("&", queryParameters.Select(x => $"{x.Key}={x.Value}"))}";
        }
    }
}