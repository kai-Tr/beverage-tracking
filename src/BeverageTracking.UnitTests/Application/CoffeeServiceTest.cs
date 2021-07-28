using BeverageTracking.API.Connectors;
using BeverageTracking.API.Context;
using BeverageTracking.API.Instrucstures.Exceptions;
using BeverageTracking.API.Repositories;
using BeverageTracking.API.Services;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BeverageTracking.UnitTests.Application
{
    public class CoffeeServiceTest
    {
        private readonly Mock<IWeatherConnector> _weatherConnectorMock;
        private readonly Mock<IServerContext> _serverContextMock;
        private readonly Mock<ICoffeeStockRepository> _coffeeStockRepository;

        public CoffeeServiceTest()
        {
            _weatherConnectorMock = new Mock<IWeatherConnector>();
            _serverContextMock = new Mock<IServerContext>();
            _coffeeStockRepository = new Mock<ICoffeeStockRepository>();
        }

        [Theory]
        [InlineData("2021-2-28", 29, "Your piping hot coffee is ready")]
        [InlineData("2021-2-28", 31, "Your refreshing iced coffee is ready")]
        public async Task Brew_Return_A_Success_Response(string preparedDateString, int temperature, string expectedMessage)
        {
            //Arrange
            var preparedDate = DateTime.Parse(preparedDateString);
            _serverContextMock.Setup(_ => _.ServerTime).Returns(preparedDate);
            _weatherConnectorMock.Setup(_ => _.GetTemperatureAsync()).ReturnsAsync(temperature);
            _coffeeStockRepository.Setup(_ => _.RemoveStock(It.IsAny<int>())).Verifiable();

            //Act
            var coffeeService = new CoffeeService(_weatherConnectorMock.Object, _serverContextMock.Object, _coffeeStockRepository.Object);
            var brewCoffeeResponse = await coffeeService.BrewAsync();

            //Assert            
            Assert.Equal(expectedMessage, brewCoffeeResponse.Message);
            Assert.Equal(preparedDate, brewCoffeeResponse.Prepared);
        }

        [Fact]
        public async Task Brew_Return_A_Throw_Teapot_Exception()
        {
            //Arrange
            _serverContextMock.Setup(_ => _.ServerTime).Returns(new DateTime(2021, 4, 1));
            _weatherConnectorMock.Setup(_ => _.GetTemperatureAsync()).ReturnsAsync(19);
            _coffeeStockRepository.Setup(_ => _.RemoveStock(It.IsAny<int>())).Verifiable();

            //Act
            var coffeeService = new CoffeeService(_weatherConnectorMock.Object, _serverContextMock.Object, _coffeeStockRepository.Object);
            Func<Task> act = () => coffeeService.BrewAsync();

            //Assert
            var ex = await Assert.ThrowsAsync<TeapotException>(act);
            Assert.Contains("I’m a teapot", ex.Message);
        }

        [Fact]
        public async Task Brew_Return_A_Throw_Out_Of_Stock_Exception()
        {
            //Arrange
            _serverContextMock.Setup(_ => _.ServerTime).Returns(new DateTime(2021, 5, 1));
            _weatherConnectorMock.Setup(_ => _.GetTemperatureAsync()).ReturnsAsync(19);
            _coffeeStockRepository.Setup(_ => _.RemoveStock(It.IsAny<int>())).Throws(new ServiceUnavailableException());

            //Act
            var coffeeService = new CoffeeService(_weatherConnectorMock.Object, _serverContextMock.Object, _coffeeStockRepository.Object);
            Func<Task> act = () => coffeeService.BrewAsync();

            //Assert
            await Assert.ThrowsAsync<ServiceUnavailableException>(act);
        }

        [Fact]
        public async Task Brew_Return_A_Server_Internal_Exception()
        {
            //Arrange
            _serverContextMock.Setup(_ => _.ServerTime).Returns(new DateTime(2021, 5, 1));
            _weatherConnectorMock.Setup(_ => _.GetTemperatureAsync()).Throws(new AppDomainException());
            _coffeeStockRepository.Setup(_ => _.RemoveStock(It.IsAny<int>())).Verifiable();

            //Act
            var coffeeService = new CoffeeService(_weatherConnectorMock.Object, _serverContextMock.Object, _coffeeStockRepository.Object);
            Func<Task> act = () => coffeeService.BrewAsync();

            //Assert
            await Assert.ThrowsAsync<AppDomainException>(act);
        }
    }
}
