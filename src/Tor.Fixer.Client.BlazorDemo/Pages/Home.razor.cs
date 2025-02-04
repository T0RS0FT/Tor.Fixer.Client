using Microsoft.AspNetCore.Components;

namespace Tor.Fixer.Client.BlazorDemo.Pages
{
    public partial class Home
    {
        [Inject]
        private IFixerClient FixerClient { get; set; }

        private string logs = string.Empty;

        private static string ApiKey
        {
            get
            {
                return Constants.FixerApiKey;
            }
            set
            {
                Constants.FixerApiKey = value;
            }
        }

        private async Task HealthCheck()
        {
            var result = await FixerClient.HealthCheckAsync();

            logs += result
                ? $"Service is available{Environment.NewLine}"
                : $"Service is not available{Environment.NewLine}";
        }
    }
}
