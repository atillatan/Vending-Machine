using Serilog;
using Machine.Application;
using Machine.Infrastructure;
using Machine.API.Configuration;
using System.Text.Json.Serialization;

string environmentName = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
Console.WriteLine($" Environment: {environmentName}");
Console.Title = $" Vending Machine - {environmentName}";
Console.WriteLine($" CurrentDirectory - {Directory.GetCurrentDirectory()}");

var appSettings = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environmentName}.json", optional: false, reloadOnChange: true)
    .Build();

string workSpaceRelativePath = appSettings.GetValue<string>("workSpace.relativepath");
Console.WriteLine($"WorkSpace Root Relative Path:{workSpaceRelativePath}");
string workSpaceRoot = Path.GetFullPath(workSpaceRelativePath);

var configuration = new ConfigurationBuilder()
    .AddConfiguration(appSettings)
    .AddJsonFile($"{workSpaceRoot}/config/logger.{environmentName}.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration, sectionName: "Serilog")
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(new WebApplicationOptions
    {
        ApplicationName = typeof(Program).Assembly.FullName,
        ContentRootPath = Directory.GetCurrentDirectory(),
        EnvironmentName = environmentName
    });

    builder.Host.UseSerilog();

    // Add services to the container.

    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructureServices(configuration);
    builder.Services.AddSwaggerDocumentation(configuration);
    builder.Services.AddHealthChecks();
    builder.Services.AddCors(options => { options.AddPolicy("CorsPolicy", builder => builder.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed((host) => true).AllowCredentials()); });
    builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.UseApplicationLifetime(app.Lifetime);    
    app.UseSwaggerDocumentation(configuration);
    app.UseSwaggerUI();
    app.UseExceptionHandler("/error");
    app.UseHealthChecks("/health");
    app.UseCors("CorsPolicy");
    app.UseRouting();
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (System.Exception e)
{
    Log.Fatal(e, "Vending Machine API failed");
    Log.CloseAndFlush();
}