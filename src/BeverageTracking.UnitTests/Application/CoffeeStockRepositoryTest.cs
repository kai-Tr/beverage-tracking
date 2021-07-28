using BeverageTracking.API;
using BeverageTracking.API.Instrucstures.Exceptions;
using BeverageTracking.API.Repositories;
using MemoryCache.Testing.Moq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using System;
using Xunit;

namespace BeverageTracking.UnitTests.Application
{
    public class CoffeeStockRepositoryTest
    {
        private readonly Mock<IOptions<AppSettings>> _appSettingsMock;

        public CoffeeStockRepositoryTest()
        {
            _appSettingsMock = new Mock<IOptions<AppSettings>>();
        }

        [Fact]
        public void CoffeeStock_Initial()
        {
            //Arrange
            var expectedValue = 4;
            _appSettingsMock.Setup(z => z.Value)
                .Returns(new AppSettings() { ServiceCapacity = expectedValue });

            var mockedCache = Create.MockedMemoryCache();
            //Act
            var repository = new CoffeeStockRepository(mockedCache, _appSettingsMock.Object);

            //Assert
            Assert.Equal(expectedValue, repository.CoffeeStock);
        }

        [Fact]
        public void CoffeeStock_Rẹmove_Success()
        {
            //Arrange
            var initialValue = 4;
            var quantityDesired = 1;

            var mockedCache = Create.MockedMemoryCache();
            mockedCache.Set(CoffeeStockRepository.CacheKey, initialValue);
            //Act
            var repository = new CoffeeStockRepository(mockedCache, _appSettingsMock.Object);
            repository.RemoveStock(quantityDesired);
            //Assert
            Assert.Equal(initialValue - quantityDesired, repository.CoffeeStock);
        }

        [Fact]
        public void CoffeeStock_Rẹmove_Out_Of_Stock()
        {
            //Arrange
            var expectedValue = 4;
            _appSettingsMock.Setup(z => z.Value)
                .Returns(new AppSettings() { ServiceCapacity = expectedValue });

            var mockedCache = Create.MockedMemoryCache();
            mockedCache.Set(CoffeeStockRepository.CacheKey, 0);
            //Act
            var repository = new CoffeeStockRepository(mockedCache, _appSettingsMock.Object);
            Action act = () => repository.RemoveStock(1);

            //Assert
            var ex = Assert.Throws<ServiceUnavailableException>(act);
            Assert.Equal($"Empty stock, the coffee machine is out of coffee", ex.Message);
            Assert.Equal(expectedValue, repository.CoffeeStock);
        }
    }
}
