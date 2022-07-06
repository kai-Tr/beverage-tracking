using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using System.Reflection;

namespace BeverageTracking.API
{
    public static class Program
    {
        static readonly string Namespace = typeof(Startup).Namespace;
        static readonly string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
        public static string EnvironmentName => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

        public static int Main(string[] args)
        {
            var configuration = GetConfiguration();
            Log.Logger = CreateSerilogLogger(configuration);
            try
            {
                Log.Information("Configuring web host ({ApplicationContext})...", Program.AppName);
                var host = CreateHostBuilder(configuration, args);
                Log.Information("Starting web host ({ApplicationContext})...", Program.AppName);
                host.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", Program.AppName);
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IConfiguration GetConfiguration()
        {
            var dict = Environment.GetEnvironmentVariables();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{EnvironmentName}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables(prefix: "PREFIX_");

            var appAssembly = Assembly.Load(new AssemblyName(AppName));
            builder.AddUserSecrets(appAssembly, optional: true);

            return builder.Build();
        }

        public static IWebHost CreateHostBuilder(IConfiguration configuration, string[] args) =>
              WebHost.CreateDefaultBuilder(args)
                  .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
                  .CaptureStartupErrors(false)
                  .UseStartup<Startup>()
                  .UseContentRoot(Directory.GetCurrentDirectory())
                  .UseSerilog()
                  .Build();

        public static ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext", Program.AppName)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
    }
}
