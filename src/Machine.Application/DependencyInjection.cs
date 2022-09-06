global using System;
global using System.Linq;
global using System.Data;
global using System.Collections.Generic;
global using System.Threading;
global using System.Threading.Tasks;
global using MediatR;
using Machine.Application.Common.Behaviours;
using Machine.Application.Languages.Commands.LanguageCreate;
namespace Machine.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(LanguageCreateCommandValidator).Assembly);
        services.AddMediatR(typeof(LanguageCreateCommand));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));        
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        return services;
    }
}