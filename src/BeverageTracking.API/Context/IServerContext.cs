using System;

namespace BeverageTracking.API.Context
{
    public interface IServerContext
    {
        /// <summary>
        /// get server date time
        /// </summary>
        DateTime ServerTime { get; }
    }
}
