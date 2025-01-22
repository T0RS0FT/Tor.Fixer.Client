using Microsoft.AspNetCore.Components;
using Tor.Fixer.Client.BlazorDemo.Extensions;
using Tor.Fixer.Client.Models;

namespace Tor.Fixer.Client.BlazorDemo.Pages
{
    public partial class LatestRates
    {
        [Inject]
        private IFixerClient FixerClient { get; set; }

        private string baseCurrencyCode = string.Empty;
        private string destinationCurrencyCodes = string.Empty;

        private LatestRatesResult latestRates;
        private string error = string.Empty;
        private bool hasError = false;
        private bool hasData = false;

        private async Task LoadData()
        {
            if (string.IsNullOrWhiteSpace(Constants.FixerApiKey))
            {
                latestRates = null;
                hasData = false;
                error = "API key required";
                hasError = true;

                return;
            }

            var destinationCodes = destinationCurrencyCodes?
                .Split(",")
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .ToArray() ?? [];

            var response = await FixerClient.GetLatestRatesAsync(baseCurrencyCode, destinationCodes);

            latestRates = response.Result;
            hasData = latestRates != null;
            error = response.Success ? string.Empty : response.Error.ToMessage();
            hasError = !string.IsNullOrWhiteSpace(error);
        }
    }
}