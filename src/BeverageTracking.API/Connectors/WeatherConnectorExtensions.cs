using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Caching;
using Polly.Registry;
using System;
using System.Net;

namespace BeverageTracking.API.Connectors
{
    public static class WeatherConnectorExtensions
    {
        public static void AddWeatherConnector(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<OpenWeatherOptions>(configuration.GetSection("OpenWeatherOptions"));
            services.AddScoped<IWeatherConnector, OpenWeatherMapConnector>();
            services.AddHttpClient<IWeatherClient, WeatherClient>()
                     .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                     .AddTransientHttpErrorPolicy(builder =>
                        builder.OrResult(res => res.StatusCode == HttpStatusCode.NotFound || res.StatusCode == HttpStatusCode.ServiceUnavailable)
                            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

            services.AddMemoryCache();
            services.AddSingleton<IAsyncCacheProvider, Polly.Caching.Memory.MemoryCacheProvider>();
            services.AddSingleton<IReadOnlyPolicyRegistry<string>, PolicyRegistry>((serviceProvider) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<OpenWeatherOptions>>().Value;
                PolicyRegistry registry = new PolicyRegistry
                {
                    {
                        OpenWeatherMapConnector.WeatherConnectorCachePolicyName,
                        Policy.CacheAsync(serviceProvider
                                            .GetRequiredService<IAsyncCacheProvider>()
                                            .AsyncFor<double>(), TimeSpan.FromMinutes(options.TTL))
                    }
                };
                return registry;
            });
        }
    }
}
