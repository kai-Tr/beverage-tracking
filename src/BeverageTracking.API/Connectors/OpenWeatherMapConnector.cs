using Polly;
using Polly.Registry;
using System.Threading.Tasks;

namespace BeverageTracking.API.Connectors
{
    public class OpenWeatherMapConnector : IWeatherConnector
    {
        public const string WeatherConnectorCachePolicyName = "WeatherConnectorCachePolicy";
        public const string WeatherConnectorCacheKeyName = "WeatherConnectorCacheKey";

        private readonly IWeatherClient _client;
        private readonly IAsyncPolicy<double> _cachePolicy;

        public OpenWeatherMapConnector(IWeatherClient client, IReadOnlyPolicyRegistry<string> policyRegistry)
        {
            _cachePolicy = policyRegistry.Get<IAsyncPolicy<double>>(WeatherConnectorCachePolicyName);
            _client = client;
        }

        public async Task<double> GetTemperatureAsync()
        {
            return await _cachePolicy.ExecuteAsync(context => _client.CallToOpenWeatherAsync(), new Polly.Context(WeatherConnectorCacheKeyName));
        }
    }
}
