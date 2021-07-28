namespace BeverageTracking.API.Connectors
{
    public class OpenWeatherOptions
    {
        public string Url { get; set; }
        public string ApiId { get; set; }
        public int TTL { get; set; }
        public string City { get; set; }
    }
}
