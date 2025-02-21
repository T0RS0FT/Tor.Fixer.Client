using Tor.Fixer.Client.Enums;

namespace Tor.Fixer.Client
{
    public class FixerOptions
    {
        public string BaseUrl { get; private set; }

        public string ApiKey { get; private set; }

        public Func<string> ApiKeyFactory { get; private set; }

        public HttpErrorHandlingMode HttpErrorHandlingMode { get; private set; } = HttpErrorHandlingMode.ReturnsError;

        public FixerOptions WithBaseUrl(string baseUrl)
        {
            BaseUrl = baseUrl;

            return this;
        }

        public FixerOptions WithApiKey(string apiKey)
        {
            if (ApiKeyFactory != null)
            {
                throw new Exception("You can not set the ApiKey and the ApiKeyFactory at the same time");
            }

            ApiKey = apiKey;

            return this;
        }

        public FixerOptions WithApiKeyFactory(Func<string> apiKeyFactory)
        {
            if (!string.IsNullOrWhiteSpace(ApiKey))
            {
                throw new Exception("You can not set the ApiKey and the ApiKeyFactory at the same time");
            }

            ApiKeyFactory = apiKeyFactory;

            return this;
        }

        public FixerOptions WithHttpErrorHandling(HttpErrorHandlingMode httpErrorHandlingMode)
        {
            HttpErrorHandlingMode = httpErrorHandlingMode;

            return this;
        }
    }
}