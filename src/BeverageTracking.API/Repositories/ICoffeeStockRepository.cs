using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeverageTracking.API.Repositories
{
    public interface ICoffeeStockRepository
    {
        public int CoffeeStock { get; }
        public void RemoveStock(int quantityDesired);
    }
}
