using BeverageTracking.API.Connectors;
using BeverageTracking.API.Instrucstures.Exceptions;
using Microsoft.Extensions.Options;
using Moq;
using Polly;
using Polly.Caching;
using Polly.Registry;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BeverageTracking.UnitTests.Application
{
    public class OpenWeatherMapConnectorTest
    {
        private readonly Mock<IWeatherClient> _clientMock;
        private readonly Mock<IAsyncCacheProvider> _cacheCacheProviderMock;

        public OpenWeatherMapConnectorTest()
        {
            _clientMock = new Mock<IWeatherClient>();
            _cacheCacheProviderMock = new Mock<IAsyncCacheProvider>();
        }

        [Fact]
        public async Task Get_Temperature_Success()
        {
            //Arrange
            var expectedTemperature = 20;
            _clientMock.Setup(p => p.CallToOpenWeatherAsync(It.IsAny<string>())).ReturnsAsync(expectedTemperature);
            var cachePolicy = Policy.CacheAsync(_cacheCacheProviderMock.Object.AsyncFor<double>(), TimeSpan.FromMinutes(1));

            var registryReturningMockPolicy = new PolicyRegistry {
                { OpenWeatherMapConnector.WeatherConnectorCachePolicyName, cachePolicy }
            };

            //Act
            var openWeatherConnector = new OpenWeatherMapConnector(_clientMock.Object, registryReturningMockPolicy);
            var temperature = await openWeatherConnector.GetTemperatureAsync();

            //Assert
            Assert.Equal(expectedTemperature, temperature);
        }

        [Fact]
        public async Task Get_Temperature_Throw_Exception()
        {
            //Arrange
            _clientMock.Setup(p => p.CallToOpenWeatherAsync(It.IsAny<string>())).ThrowsAsync(new AppDomainException());
            var cachePolicy = Policy.CacheAsync(_cacheCacheProviderMock.Object.AsyncFor<double>(), TimeSpan.FromMinutes(1));

            var registryReturningMockPolicy = new PolicyRegistry {
                { OpenWeatherMapConnector.WeatherConnectorCachePolicyName, cachePolicy }
            };

            //Act
            var openWeatherConnector = new OpenWeatherMapConnector(_clientMock.Object, registryReturningMockPolicy);
            Func<Task> act = () => openWeatherConnector.GetTemperatureAsync();

            //Assert
            await Assert.ThrowsAsync<AppDomainException>(act);
        }
    }
}
