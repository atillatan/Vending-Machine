
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Machine.Application.Common.Interface;
using Machine.Infrastructure.Services;
using Machine.Infrastructure.Persistence;
using Machine.Application.Common.Interfaces;
using Machine.Infrastructure.Services.Languages;
using vmachine;
using Serilog;
using Serilog.Events;

// Initialize Configuration
IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(AppDomain.CurrentDomain.BaseDirectory + "/appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"{Directory.GetCurrentDirectory()}/config/logger.Development.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration, sectionName: "Serilog")  
    .CreateLogger();

// Create ServiceCollection for required services
ServiceProvider svcProvider = new ServiceCollection()
                .AddSingleton<IConfiguration>(configuration)
                .AddDbContext<ApplicationDbContext>()
                .AddSingleton<ILanguageService>(x => new LanguageService(configuration, x.GetService<ApplicationDbContext>()!))
                .AddSingleton<IVendingMachine>(x => new VendingMachineService(configuration, x.GetService<ApplicationDbContext>()!))                
                .BuildServiceProvider();

// Build RootCommand
var root = new VendingMachineCommand(svcProvider).BuildRootCommand();
root.InvokeAsync(args);
