using System.Threading.Tasks;

namespace BeverageTracking.API.Connectors
{
    public interface IWeatherConnector
    {
        Task<double> GetTemperatureAsync();
    }
}
