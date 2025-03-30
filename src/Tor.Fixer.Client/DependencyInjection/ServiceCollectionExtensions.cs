using Microsoft.Extensions.DependencyInjection;
using Tor.Fixer.Client.Internal;

namespace Tor.Fixer.Client.DependencyInjection
{
    public static class ServiceCollectionExtensions
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