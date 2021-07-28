using BeverageTracking.API.Connectors;
using BeverageTracking.API.Instrucstures.Exceptions;
using Microsoft.Extensions.Options;
using Moq;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BeverageTracking.UnitTests.Application
{
    public class WeatherClientTest
    {
        private readonly Mock<IOptions<OpenWeatherOptions>> _weatherOptionsMock;

        public WeatherClientTest()
        {
            _weatherOptionsMock = new Mock<IOptions<OpenWeatherOptions>>();
            _weatherOptionsMock.Setup(z => z.Value)
                .Returns(new OpenWeatherOptions() { ApiId = "123456", Url = "http://localhost/api/weather", TTL = 120 });
        }

        [Fact]
        public async Task Call_To_Open_Weather_Success()
        {
            //Arrange
            var city = "London,uk";
            var expectedTemperature = 17.74;
            var mockHttp = new MockHttpMessageHandler();
            var options = _weatherOptionsMock.Object.Value;
            mockHttp.When($"{options.Url.TrimEnd('/')}?q={city}&units=metric&appId={options.ApiId}")
                            .Respond("application/json", $"{{'main' : {{ 'temp': {expectedTemperature} }} }}");

            var client = mockHttp.ToHttpClient();

            //Act
            var openWeatherConnector = new WeatherClient(client, _weatherOptionsMock.Object);
            var temperature = await openWeatherConnector.CallToOpenWeatherAsync(city);
            //Assert
            Assert.Equal(expectedTemperature, temperature);
        }

        [Fact]
        public async Task Call_To_Open_Weather_Return_HttpRequestException()
        {
            //Arrange
            var city = "London,uk";
            var mockHttp = new MockHttpMessageHandler();
            var options = _weatherOptionsMock.Object.Value;
            mockHttp.When($"{options.Url.TrimEnd('/')}?q={city}&units=metric&appId={options.ApiId}")
                            .Respond(HttpStatusCode.Unauthorized, "application/json", "{ 'code': 401, 'message' : 'Invalid API key' }");                           

            var client = mockHttp.ToHttpClient();

            //Act
            var openWeatherConnector = new WeatherClient(client, _weatherOptionsMock.Object);
            Func<Task> act = () => openWeatherConnector.CallToOpenWeatherAsync(city);

            //Assert
            var ex = await Assert.ThrowsAsync<AppDomainException>(act);
            Assert.Contains("Invalid API key", ex.Message);
        }

        [Fact]
        public async Task Call_To_Open_Weather_Return_UnknownException()
        {
            //Arrange
            var city = "London,uk";
            var mockHttp = new MockHttpMessageHandler();
            var options = _weatherOptionsMock.Object.Value;
            mockHttp.When($"{options.Url.TrimEnd('/')}?q={city}&units=metric&appId={options.ApiId}")
                            .Throw(new Exception("Timeout"));

            var client = mockHttp.ToHttpClient();

            //Act
            var openWeatherConnector = new WeatherClient(client, _weatherOptionsMock.Object);
            Func<Task> act = () => openWeatherConnector.CallToOpenWeatherAsync(city);

            //Assert
            var ex = await Assert.ThrowsAsync<AppDomainException>(act);
            Assert.Contains("Timeout", ex.Message);
        }
    }
}
