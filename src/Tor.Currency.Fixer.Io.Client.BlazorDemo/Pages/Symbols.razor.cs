using Microsoft.AspNetCore.Components;
using Tor.Currency.Fixer.Io.Client.BlazorDemo.Extensions;
using Tor.Currency.Fixer.Io.Client.Models;

namespace Tor.Currency.Fixer.Io.Client.BlazorDemo.Pages
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
            if (string.IsNullOrWhiteSpace(Constants.FixerApiKey))
            {
                this.symbols = [];
                this.hasData = false;
                this.error = "API key required";
                this.hasError = true;

                return;
            }

            var response = await this.FixerClient.GetSymbolsAsync();

            this.symbols = response.Result ?? [];
            this.hasData = this.symbols.Count > 0;
            this.error = response.Success ? string.Empty : response.Error.ToMessage();
            this.hasError = !string.IsNullOrWhiteSpace(this.error);
        }
    }
}
