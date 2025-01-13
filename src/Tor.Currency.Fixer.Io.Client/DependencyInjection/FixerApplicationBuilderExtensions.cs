using Microsoft.Extensions.DependencyInjection;

namespace Tor.Currency.Fixer.Io.Client.DependencyInjection
{
    public static class FixerServiceCollectionExtensions
    {
        public static void AddFixer(this IServiceCollection services, Action<FixerOptions> fixerOptions)
        {
            services.AddScoped<IFixerClient, FixerClient>();

            services.AddHttpClient<FixerClient>(options =>
            {
                options.BaseAddress = new Uri("http://data.fixer.io/api/");
            });

            services.Configure(fixerOptions);
        }
    }
}