using Microsoft.AspNetCore.Components;
using Tor.Currency.Fixer.Io.Client.Models;

namespace Tor.Currency.Fixer.Io.Client.BlazorDemo.Pages
{
    public partial class Symbols
    {
        [Inject]
        private IFixerClient FixerClient { get; set; }

        private List<Symbol> symbols = [];
        private string error = string.Empty;
        private bool hasError = false;
        private bool hasSymbols = false;

        private async Task LoadSymbols()
        {
            if (string.IsNullOrWhiteSpace(Constants.FixerApiKey))
            {
                this.symbols = [];
                this.hasSymbols = false;
                this.error = "API key required";
                this.hasError = true;

                return;
            }

            var response = await this.FixerClient.GetSymbolsAsync();

            this.symbols = response.Data ?? [];
            this.hasSymbols = this.symbols.Count > 0;
            this.error = response.Success ? string.Empty : $"{response.Error.Info} ({response.Error.Code})";
            this.hasError = !string.IsNullOrWhiteSpace(this.error);
        }
    }
}
