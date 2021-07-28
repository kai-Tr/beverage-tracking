using BeverageTracking.API;
using BeverageTracking.API.Controllers;
using BeverageTracking.API.Instrucstures.ActionResults;
using BeverageTracking.API.Instrucstures.Exceptions;
using BeverageTracking.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BeverageTracking.UnitTests.Application
{
    public class CoffeeControllerTest
    {
        private readonly Mock<ICoffeeService> _coffeeServiceMock;
        public CoffeeControllerTest()
        {
            _coffeeServiceMock = new Mock<ICoffeeService>();
        }

        [Fact]
        public async Task Get_Brew_Coffee_Success_With_Less_Temperature()
        {
            var preparedDate = new DateTime(2021, 2, 28);
            var response = new BrewCoffeeResponse(preparedDate, 29);

            //Arrange
            _coffeeServiceMock.Setup(x => x.BrewAsync()).ReturnsAsync(response);

            //Act
            var coffeeController = new CoffeeController(_coffeeServiceMock.Object);
            var actionResult = await coffeeController.BrewCoffee();

            //Assert
            var result = Assert.IsType<OkObjectResult>(actionResult);
            var resultValue = Assert.IsType<BrewCoffeeResponse>(result.Value);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal("Your piping hot coffee is ready", resultValue.Message);
            Assert.Equal(preparedDate, resultValue.Prepared);
        }

        [Fact]
        public async Task Get_Brew_Coffee_Success_With_Greater_Temperature()
        {
            var preparedDate = new DateTime(2021, 2, 28);
            var response = new BrewCoffeeResponse(preparedDate, 31);

            //Arrange
            _coffeeServiceMock.Setup(x => x.BrewAsync()).ReturnsAsync(response);

            //Act
            var coffeeController = new CoffeeController(_coffeeServiceMock.Object);
            var actionResult = await coffeeController.BrewCoffee();

            //Assert
            var result = Assert.IsType<OkObjectResult>(actionResult);
            var resultValue = Assert.IsType<BrewCoffeeResponse>(result.Value);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal("Your refreshing iced coffee is ready", resultValue.Message);
            Assert.Equal(preparedDate, resultValue.Prepared);
        }

        [Fact]
        public async Task Get_Brew_Coffee_Return_Service_Unavailable()
        {
            //Arrange
            _coffeeServiceMock.Setup(x => x.BrewAsync()).Throws(new ServiceUnavailableException());

            //Act
            var coffeeController = new CoffeeController(_coffeeServiceMock.Object);
            var actionResult = await coffeeController.BrewCoffee();

            //Assert
            var result = Assert.IsType<ServiceUnavailableErrorResult>(actionResult);
            Assert.Equal(StatusCodes.Status503ServiceUnavailable, result.StatusCode);
        }

        [Fact]
        public async Task Get_Brew_Coffee_Return_Teapot()
        {
            //Arrange
            _coffeeServiceMock.Setup(x => x.BrewAsync()).Throws(new TeapotException());

            //Act
            var coffeeController = new CoffeeController(_coffeeServiceMock.Object);
            var actionResult = await coffeeController.BrewCoffee();

            //Assert
            var result = Assert.IsType<TeapotErrorResult>(actionResult);
            Assert.Equal(StatusCodes.Status418ImATeapot, result.StatusCode);
        }
    }
}
