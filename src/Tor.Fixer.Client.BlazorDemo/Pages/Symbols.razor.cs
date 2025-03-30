using Microsoft.AspNetCore.Components;
using Tor.Fixer.Client.BlazorDemo.Extensions;
using Tor.Fixer.Client.Models;

namespace Tor.Fixer.Client.BlazorDemo.Pages
{
    public partial class Symbols
    {
        [Inject]
        private IFixerClient FixerClient { get; set; }

        private List<SymbolResult> symbols = [];
        private string error = string.Empty;
        private bool hasError = false;
        private bool hasData = false;

        private async Task LoadData()
        {
            if (string.IsNullOrWhiteSpace(Constants.ApiKey))
            {
                symbols = [];
                hasData = false;
                error = "API key required";
                hasError = true;

                return;
            }

            var response = await FixerClient.GetSymbolsAsync();

            symbols = response.Result ?? [];
            hasData = symbols.Count > 0;
            error = response.Success ? string.Empty : response.Error.ToMessage();
            hasError = !string.IsNullOrWhiteSpace(error);
        }
    }
}
