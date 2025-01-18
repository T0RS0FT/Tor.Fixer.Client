using Microsoft.AspNetCore.Components;
using Tor.Currency.Fixer.Io.Client.BlazorDemo.Extensions;

namespace Tor.Currency.Fixer.Io.Client.BlazorDemo.Pages
{
    public partial class HistoricalRates
    {
        [Inject]
        private IFixerClient FixerClient { get; set; }

        private string baseCurrencyCode = string.Empty;
        private string destinationCurrencyCodes = string.Empty;
        private DateOnly date = DateOnly.FromDateTime(DateTime.Now);

        private Models.HistoricalRates historicalRates;
        private string error = string.Empty;
        private bool hasError = false;
        private bool hasData = false;

        private async Task LoadData()
        {
            if (string.IsNullOrWhiteSpace(Constants.FixerApiKey))
            {
                this.historicalRates = null;
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

            var response = await this.FixerClient.GetHistoricalRatesAsync(this.date, this.baseCurrencyCode, destinationCodes);

            this.historicalRates = response.Data;
            this.hasData = this.historicalRates != null;
            this.error = response.Success ? string.Empty : response.Error.ToMessage();
            this.hasError = !string.IsNullOrWhiteSpace(this.error);
        }
    }
}
