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
            var httpResponse = await httpClient.GetAsync($"symbols?access_key={this.GetApiKey()}");

            var fixerResponse = GetFixerResponse<List<Symbol>>(httpResponse, options.Value);

            if (!fixerResponse.Success)
            {
                return fixerResponse;
            }

            var content = await httpResponse.Content.ReadFromJsonAsync<SymbolsModel>();

            fixerResponse.Success = content.Success;
            fixerResponse.Error = content.Error.ToFixerError();
            fixerResponse.Data = content.Symbols
                .Select(x => new Symbol() { Code = x.Key, Name = x.Value })
                .ToList();

            return fixerResponse;
        }

        private string GetApiKey()
        {
            var apiKey = options.Value.ApiKeyFactory?.Invoke() ?? options.Value.ApiKey;

            return !string.IsNullOrWhiteSpace(apiKey)
                ? apiKey
                : throw new Exception("API key required");
        }

        private static FixerResponse<T> GetFixerResponse<T>(HttpResponseMessage httpResponse, FixerOptions options)
        {
            switch (options.HttpErrorHandlingMode)
            {
                case HttpErrorHandlingMode.ThrowException:
                    httpResponse.EnsureSuccessStatusCode();
                    break;
                case HttpErrorHandlingMode.ReturnError:
                    if (!httpResponse.IsSuccessStatusCode)
                    {
                        return new FixerResponse<T>()
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

            return new FixerResponse<T>();
        }
    }
}