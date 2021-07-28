using BeverageTracking.API;
using BeverageTracking.API.Context;
using BeverageTracking.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using Xunit;

namespace BeverageTracking.UnitTests.Application
{
    public class CoffeeControllerTest
    {
        private readonly Mock<IServerContext> _serverContextMock;
        public CoffeeControllerTest()
        {
            _serverContextMock = new Mock<IServerContext>();
        }

        [Fact]
        public void Get_Brew_Coffee_Success()
        {
            var preparedDate = new DateTime(2021, 2, 28);
            //Arrange
            _serverContextMock.Setup(x => x.ServerTime).Returns(preparedDate);
            _serverContextMock.Setup(x => x.CoffeeStock).Returns(5);

            //Act
            var coffeeController = new CoffeeController(_serverContextMock.Object);
            var actionResult = coffeeController.BrewCoffee();

            //Assert
            var result = Assert.IsType<OkObjectResult>(actionResult);
            var resultValue = Assert.IsType<BrewCoffeeResponse>(result.Value);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal("Your piping hot coffee is ready", resultValue.Message);
            Assert.Equal(preparedDate, resultValue.Prepared);
        }

        [Fact]
        public void Get_Brew_Coffee_Return_I_Teapot()
        {
            var preparedDate = new DateTime(2021, 4, 1);
            //Arrange
            _serverContextMock.Setup(x => x.ServerTime).Returns(preparedDate);
            _serverContextMock.Setup(x => x.CoffeeStock).Returns(5);

            //Act
            var coffeeController = new CoffeeController(_serverContextMock.Object);
            var actionResult = coffeeController.BrewCoffee();

            //Assert
            var result = Assert.IsType<StatusCodeResult>(actionResult);
            Assert.Equal(StatusCodes.Status418ImATeapot, result.StatusCode);
        }

        [Fact]
        public void Get_Brew_Coffee_Return_Service_Unavailable()
        {            
            //Arrange
            _serverContextMock.Setup(x => x.CoffeeStock).Returns(0);

            //Act
            var coffeeController = new CoffeeController(_serverContextMock.Object);
            var actionResult = coffeeController.BrewCoffee();

            //Assert
            var result = Assert.IsType<StatusCodeResult>(actionResult);
            Assert.Equal(StatusCodes.Status503ServiceUnavailable, result.StatusCode);
        }
    }
}
