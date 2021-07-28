using System;

namespace BeverageTracking.API.Context
{
    public class ServerContext : IServerContext
    {
        public DateTime ServerTime => DateTime.Now;
    }
}
