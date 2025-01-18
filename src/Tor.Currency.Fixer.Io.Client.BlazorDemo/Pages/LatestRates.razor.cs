using Microsoft.AspNetCore.Components;
using Tor.Currency.Fixer.Io.Client.BlazorDemo.Extensions;

namespace Tor.Currency.Fixer.Io.Client.BlazorDemo.Pages
{
    public partial class LatestRates
    {
        [Inject]
        private IFixerClient FixerClient { get; set; }

        private string baseCurrencyCode = string.Empty;
        private string destinationCurrencyCodes = string.Empty;

        private Models.LatestRates latestRates;
        private string error = string.Empty;
        private bool hasError = false;
        private bool hasData = false;

        private async Task LoadData()
        {
            if (string.IsNullOrWhiteSpace(Constants.FixerApiKey))
            {
                this.latestRates = null;
                this.hasData = false;
                this.error = "API key required";
                this.hasError = true;

                return;
            }

            var destinationCodes = this.destinationCurrencyCodes?
                .Split(",")
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .ToArray() ?? [];

            var response = await this.FixerClient.GetLatestRatesAsync(this.baseCurrencyCode, destinationCodes);

            this.latestRates = response.Data;
            this.hasData = this.latestRates != null;
            this.error = response.Success ? string.Empty : response.Error.ToMessage();
            this.hasError = !string.IsNullOrWhiteSpace(this.error);
        }
    }
}