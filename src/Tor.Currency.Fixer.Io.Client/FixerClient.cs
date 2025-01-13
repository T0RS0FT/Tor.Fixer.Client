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
                $"symbols?access_key={this.GetApiKey()}",
                x => x.Symbols.Select(x => new Symbol() { Code = x.Key, Name = x.Value }).ToList());
        }

        private string GetApiKey()
        {
            var apiKey = options.Value.ApiKeyFactory?.Invoke() ?? options.Value.ApiKey;

            return !string.IsNullOrWhiteSpace(apiKey)
                ? apiKey
                : throw new Exception("API key required");
        }

        private async Task<FixerResponse<TResponseModel>> GetFixerResponse<TFixerModel, TResponseModel>(
            string url,
            Func<TFixerModel, TResponseModel> mapper)
            where TFixerModel : FixerModelBase
        {
            if (!string.IsNullOrWhiteSpace(options.Value.BaseUrl))
            {
                httpClient.BaseAddress = new Uri(options.Value.BaseUrl);
            }

            var httpResponse = await httpClient.GetAsync(url);

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
    }
}