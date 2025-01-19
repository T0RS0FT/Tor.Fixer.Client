using Microsoft.Extensions.DependencyInjection;
using Tor.Currency.Fixer.Io.Client.Internal;

namespace Tor.Currency.Fixer.Io.Client.DependencyInjection
{
    public static class FixerServiceCollectionExtensions
    {
        public static void AddFixer(this IServiceCollection services, Action<FixerOptions> fixerOptions)
        {
            services.AddScoped<IFixerClient, FixerClient>();

            services.AddHttpClient<IFixerClient, FixerClient>(options =>
            {
                options.BaseAddress = new Uri(Constants.DefaultFixerUrl);
            });

            services.Configure(fixerOptions);
        }
    }
}