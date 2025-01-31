using Microsoft.AspNetCore.Components;
using Tor.Fixer.Client.BlazorDemo.Extensions;
using Tor.Fixer.Client.Models;

namespace Tor.Fixer.Client.BlazorDemo.Pages
{
    public partial class Fluctuation
    {
        [Inject]
        private IFixerClient FixerClient { get; set; }

        private DateOnly startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-3).Date);
        private DateOnly endDate = DateOnly.FromDateTime(DateTime.Now.Date);
        private string baseCurrencyCode = string.Empty;
        private string destinationCurrencyCodes = string.Empty;

        private FluctuationResult fluctuation;
        private string error = string.Empty;
        private bool hasError = false;
        private bool hasData = false;

        private async Task LoadData()
        {
            if (string.IsNullOrWhiteSpace(Constants.FixerApiKey))
            {
                fluctuation = null;
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

            var response = await FixerClient.GetFluctuationAsync(startDate, endDate, baseCurrencyCode, destinationCodes);

            fluctuation = response.Result;
            hasData = fluctuation != null;
            error = response.Success ? string.Empty : response.Error.ToMessage();
            hasError = !string.IsNullOrWhiteSpace(error);
        }
    }
}
