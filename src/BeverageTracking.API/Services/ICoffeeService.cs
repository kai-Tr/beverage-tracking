using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeverageTracking.API.Services
{
    public interface ICoffeeService
    {
        /// <summary>
        /// brew coffee
        /// </summary>
        Task<BrewCoffeeResponse> BrewAsync();
    }

    public interface IBreakfastCoffeeService : ICoffeeService
    {
        void Test();
    }
}
