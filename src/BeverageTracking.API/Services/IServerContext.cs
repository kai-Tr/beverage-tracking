using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeverageTracking.API.Context
{
    public interface IServerContext
    {
        DateTime ServerTime { get; }
        int CoffeeStock { get; set; }
    }
}
