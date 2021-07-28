using BeverageTracking.API.Instrucstures.Exceptions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace BeverageTracking.API.Repositories
{
    public class CoffeeStockRepository : ICoffeeStockRepository
    {
        private readonly IMemoryCache _cache;
        private readonly AppSettings _appSettings;
        public const string CacheKey = "CoffeeStockKey";
        public CoffeeStockRepository(IMemoryCache memoryCache, IOptions<AppSettings> options)
        {
            _cache = memoryCache;
            _appSettings = options.Value;
        }

        public int CoffeeStock
        {
            get
            {
                // Look for cache key.
                return _cache.GetOrCreate(CacheKey, entry =>
                {
                    return _appSettings.ServiceCapacity;
                });
            }
        }

        public void RemoveStock(int quantityDesired)
        {
            var coffeeStock = CoffeeStock;
            if (coffeeStock == 0)
            {
                ResetStock();
                throw new ServiceUnavailableException($"Empty stock, the coffee machine is out of coffee");
            }
            _cache.Set(CacheKey, coffeeStock - quantityDesired);
        }

        private void ResetStock()
        {
            _cache.Set(CacheKey, _appSettings.ServiceCapacity);
        }
    }
}
