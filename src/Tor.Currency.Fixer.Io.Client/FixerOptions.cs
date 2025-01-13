using Tor.Currency.Fixer.Io.Client.Enums;

namespace Tor.Currency.Fixer.Io.Client
{
    public class FixerOptions
    {
        public string BaseUrl { get; set; } = "https://data.fixer.io/api/";

        public string ApiKey { get; private set; }

        public Func<string> ApiKeyFactory { get; private set; }

        public HttpErrorHandlingMode HttpErrorHandlingMode { get; private set; } = HttpErrorHandlingMode.ReturnError;

        public FixerOptions WithBaseUrl(string baseUrl)
        {
            this.BaseUrl = baseUrl;

            return this;
        }

        public FixerOptions WithApiKey(string apiKey)
        {
            if (this.ApiKeyFactory != null)
            {
                throw new Exception("You can not set the ApiKey and the ApiKeyFactory at the same time");
            }

            this.ApiKey = apiKey;

            return this;
        }

        public FixerOptions WithApiKeyFactory(Func<string> apiKeyFactory)
        {
            if (!string.IsNullOrWhiteSpace(this.ApiKey))
            {
                throw new Exception("You can not set the ApiKey and the ApiKeyFactory at the same time");
            }

            this.ApiKeyFactory = apiKeyFactory;

            return this;
        }

        public FixerOptions WithHttpErrorHandling(HttpErrorHandlingMode httpErrorHandlingMode)
        {
            this.HttpErrorHandlingMode = httpErrorHandlingMode;

            return this;
        }
    }
}