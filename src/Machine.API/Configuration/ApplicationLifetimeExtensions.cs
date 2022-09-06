using Serilog;
namespace Machine.API.Configuration;

public static class ApplicationLifetimeExtensions
{
    internal static IApplicationBuilder UseApplicationLifetime(this IApplicationBuilder app, IHostApplicationLifetime appLifetime)
    {
        appLifetime.ApplicationStarted.Register(OnStarted);
        appLifetime.ApplicationStopping.Register(OnStopping);
        return app;
    }

    private static void OnStarted()
    {
        Log.Information(@"                                                           ");
        Log.Information(@"██    ██ ███████ ██     ██ ██████  ██ ███    ██  ██████    ");
        Log.Information(@"██    ██ ██      ████   ██ ██   ██ ██ ████   ██ ██         ");
        Log.Information(@"██    ██ █████   ██ ██  ██ ██   ██ ██ ██ ██  ██ ██   ███   ");
        Log.Information(@" ██  ██  ██      ██  ██ ██ ██   ██ ██ ██  ██ ██ ██    ██   ");
        Log.Information(@"  ████   ███████ ██   ████ ██████  ██ ██   ████  ██████    ");
        Log.Information(@"                                                           ");
        Log.Information(@"                                                           ");
        Log.Information(@"███    ███  █████   ██████ ██   ██ ██ ███    ██ ███████    ");
        Log.Information(@"████  ████ ██   ██ ██      ██   ██ ██ ████   ██ ██         ");
        Log.Information(@"██ ████ ██ ███████ ██      ███████ ██ ██ ██  ██ █████      ");
        Log.Information(@"██  ██  ██ ██   ██ ██      ██   ██ ██ ██  ██ ██ ██         ");
        Log.Information(@"██      ██ ██   ██  ██████ ██   ██ ██ ██   ████ ███████    ");
        Log.Information(@"                                                           ");
    }

    private static void OnStopping()
    {
        Log.Information("######  Vending Machine is shutting down...  #######");
    }
}