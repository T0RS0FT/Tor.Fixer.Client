using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Tor.Currency.Fixer.Io.Client.DependencyInjection;

namespace Tor.Currency.Fixer.Io.Client.BlazorDemo
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddFixer(options => options.WithApiKeyFactory(() => Constants.FixerApiKey));

            await builder.Build().RunAsync();
        }
    }
}
