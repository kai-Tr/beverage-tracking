using BeverageTracking.API;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BeverageTracking.FunctionalTests
{
    class StartupTestBase : Startup
    {
        public StartupTestBase(IConfiguration env) : base(env)
        {
        }

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            services.Configure<RouteOptions>(Configuration);
            base.ConfigureServices(services);
        }
    }
}
