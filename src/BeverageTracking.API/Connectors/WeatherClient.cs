using BeverageTracking.API.Instrucstures.Exceptions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BeverageTracking.API.Connectors
{
    public class WeatherClient : IWeatherClient
    {
        private readonly HttpClient _httpClient;
        private readonly OpenWeatherOptions _weatherOptions;

        public WeatherClient(HttpClient httpClient, IOptions<OpenWeatherOptions> weatherOptions)
        {
            _httpClient = httpClient;
            _weatherOptions = weatherOptions.Value;
        }

        public async Task<double> CallToOpenWeatherAsync(string city = "")
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string jsonString = null;
            city = string.IsNullOrWhiteSpace(city) ? _weatherOptions.City : city;
            try
            {
                var response = await _httpClient.GetAsync($"{_weatherOptions.Url.TrimEnd('/')}?q={city}&units=metric&appId={_weatherOptions.ApiId}");
                jsonString = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();
                dynamic result = JsonConvert.DeserializeObject(jsonString);
                return (double)result.main.temp;
            }
            catch (HttpRequestException)
            {
                var errorResult = JsonConvert.DeserializeObject<dynamic>(jsonString);
                throw new AppDomainException((string)errorResult.message);
            }
            catch (Exception ex)
            {
                throw new AppDomainException(ex.Message);
            }
        }
    }
}
