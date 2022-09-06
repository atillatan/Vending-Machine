using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
namespace Machine.API.Configuration;

internal static class SwaggerExtensions
{
    internal static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services, IConfiguration configuration)
    {
        //if (configuration.GetValue<string>("environment").Equals("Development", StringComparison.CurrentCultureIgnoreCase)) return services;

        services.AddSwaggerGen(options =>
        {

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = configuration.GetValue<string>("name", "Vending Machine"),
                Version = configuration.GetValue<string>("version", "1.0.0"),
                Contact = new OpenApiContact { Email = "atilla@tanrikulu.biz" },
                Description = configuration.GetValue<string>("name", "Vending Machine") + " REST API CQRS implementation with using DDD pattern."
            });
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, Assembly.GetAssembly(typeof(Machine.Application.DependencyInjection))?.GetName().Name + ".xml"));
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, Assembly.GetAssembly(typeof(Machine.Infrastructure.DependencyInjection))?.GetName().Name + ".xml"));
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, Assembly.GetAssembly(typeof(Machine.Domain.Entities.Product))?.GetName().Name + ".xml"));          
        });

        return services;
    }

    internal static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app, IConfiguration configuration)
    {
        // if (configuration.GetValue<string>("environment").Equals("Development", StringComparison.CurrentCultureIgnoreCase)) return app;

        app.UseSwagger(c => { c.SerializeAsV2 = true; });
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", configuration.GetValue<string>("name", "Vending Machine") + " REST API" + " " + configuration.GetValue<string>("version", "1.0.0")));
        return app;
    }
}

