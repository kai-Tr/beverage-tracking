using System.Threading.Tasks;

namespace BeverageTracking.API.Connectors
{
    public interface IWeatherClient
    {
        /// <summary>
        /// call to open weather
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        Task<double> CallToOpenWeatherAsync(string city = "");
    }
}
