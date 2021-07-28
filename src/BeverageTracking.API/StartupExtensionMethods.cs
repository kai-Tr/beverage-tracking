using BeverageTracking.API.Context;
using BeverageTracking.API.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Globalization;

namespace BeverageTracking.API
{
    public static class StartupExtensionMethods
    {
        public static IServiceCollection AddCustomMVC(this IServiceCollection services)
        {
            services.AddControllers()
            .AddApplicationPart(typeof(CoffeeController).Assembly)
            .AddControllersAsServices()
            .AddNewtonsoftJson(options =>
            {
                var dateConverter = new Newtonsoft.Json.Converters.IsoDateTimeConverter
                {
                    DateTimeFormat = "yyyy-MM-ddTHH:mm:sszzz"
                };

                options.SerializerSettings.Converters.Add(dateConverter);
                options.SerializerSettings.Culture = new CultureInfo("en-Us");
                options.SerializerSettings.Formatting = Formatting.Indented;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            return services;
        }

        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddSingleton<IServerContext, ServerContext>();
            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Beverage Tracking HTTP API",
                    Version = "v1",
                    Description = "The Beverage Tracking HTTP API. This is a testing sample"
                });
            });

            return services;

        }
    }
}
