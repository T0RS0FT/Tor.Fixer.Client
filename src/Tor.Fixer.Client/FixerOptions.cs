using Tor.Fixer.Client.Enums;

namespace Tor.Fixer.Client
{
    public class FixerOptions
    {
        public string BaseUrl { get; private set; }

        public string ApiKey { get; private set; }

        public Func<string> ApiKeyFactory { get; private set; }

        public ErrorHandlingMode HttpErrorHandlingMode { get; private set; } = ErrorHandlingMode.ReturnsError;

        public ErrorHandlingMode OtherErrorHandlingMode { get; private set; } = ErrorHandlingMode.ReturnsError;

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

        public FixerOptions WithHttpErrorHandling(ErrorHandlingMode httpErrorHandlingMode)
        {
            HttpErrorHandlingMode = httpErrorHandlingMode;

            return this;
        }

        public FixerOptions WithOtherErrorHandling(ErrorHandlingMode otherErrorHandlingMode)
        {
            OtherErrorHandlingMode = otherErrorHandlingMode;

            return this;
        }
    }
}