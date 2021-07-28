using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using BeverageTracking.API.Context;
using System;
using System.Linq;
using Autofac;
using Autofac.Extensions.DependencyInjection;

namespace BeverageTracking.FunctionalTests
{
    public class ServerTestBase
    {
        public TestServer CreateServer(DateTime? serverTime = null)
        {
            var path = Assembly.GetAssembly(typeof(ServerTestBase))
               .Location;

            var hostBuilder = new HostBuilder()
                        .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                        .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<StartupTestBase>().UseTestServer())
                        .UseEnvironment("Development")
                        .ConfigureAppConfiguration((host, builder) =>
                        {
                            builder.SetBasePath(Path.GetDirectoryName(path));
                            builder.AddJsonFile("appsettings.json", optional: true);
                            builder.AddJsonFile($"appsettings.{host.HostingEnvironment.EnvironmentName}.json", optional: true);
                            builder.AddEnvironmentVariables();

                            var appAssembly = Assembly.Load(new AssemblyName(host.HostingEnvironment.ApplicationName));
                            builder.AddUserSecrets(appAssembly, optional: true);
                        })
                        .ConfigureServices((hostingContext, services) =>
                        {
                            var serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IServerContext));
                            if (serviceDescriptor != null)
                            {
                                services.Remove(serviceDescriptor);
                            }

                            services.AddSingleton<IServerContext, ServerTestContext>(sp =>
                            {
                                return new ServerTestContext(serverTime ?? DateTime.Now);
                            });
                        }).Build();

            hostBuilder.StartAsync();
            return hostBuilder.GetTestServer();
        }
    }

    public class ServerTestContext : IServerContext
    {
        public ServerTestContext(DateTime serverTime)
        {
            _serverTime = serverTime;
        }

        private readonly DateTime _serverTime;
        public DateTime ServerTime
        {
            get
            {
                return _serverTime;
            }
        }

        public int CoffeeStock { get; set; } = 4;
    }
}
