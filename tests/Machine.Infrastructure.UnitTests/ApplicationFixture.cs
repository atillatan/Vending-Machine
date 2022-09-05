using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using Microsoft.AspNetCore.Builder;
using System.Text.Json.Serialization;
using Machine.Infrastructure.Persistence;
using Machine.Application.Common.Interfaces;
using Machine.Application.Common.Interface;
using Machine.Infrastructure.Services.Languages;
using Machine.Infrastructure.Services;
using FluentAssertions;

namespace Machine.Infrastructure.UnitTests;

public class ApplicationFixture : IDisposable
{
    public IConfiguration Configuration { get; private set; }

    public ServiceProvider SvcProvider { get; private set; }

    public static int count = 0;
    public ApplicationFixture()
    {
        try
        {
            // Initialize Configuration
            Configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory()) 
                        .AddJsonFile(AppDomain.CurrentDomain.BaseDirectory + "../../../../../src/Machine.CLI/appsettings.test.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables()
                        .Build();

            // Create ServiceCollection for required services
            SvcProvider = new ServiceCollection()
                           .AddSingleton<IConfiguration>(Configuration)
                           .AddDbContext<ApplicationDbContext>()
                           .AddSingleton<ILanguageService>(x => new LanguageService(Configuration, x.GetService<ApplicationDbContext>()!))
                           .AddSingleton<IVendingMachine>(x => new VendingMachineService(Configuration, x.GetService<ApplicationDbContext>()!))
                           .BuildServiceProvider();

                                       
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    ~ApplicationFixture()
    {
        Dispose();
    }

    public void Dispose()
    {
        SvcProvider.Dispose();
        GC.SuppressFinalize(this);
    }
}