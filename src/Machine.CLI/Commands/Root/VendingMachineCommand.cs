using Machine.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Machine.Commands.Root;

public class VendingMachineCommand
{
    private IServiceProvider? _svcProvider { get; set; } = default!;

    private IConfiguration? _configuration { get; set; } = default!;

    private ILanguageService _langSvc { get; set; } = default!;

    public VendingMachineCommand(IServiceProvider serviceProvider)
    {
        this._svcProvider = serviceProvider;
        this._configuration = _svcProvider!.GetRequiredService<IConfiguration>();
    }

    /// <summary>
    /// Creates the new RootCommand for CLI application
    /// </summary>
    /// <returns>RootCommand</returns>
    public RootCommand BuildRootCommand()
    {
        // Read configuration
        string defaultCurrency = _configuration.GetValue<string>("default.currency", "EUR");
        string defaultLanguage = _configuration.GetValue<string>("default.language", "EN");
        string name = _configuration.GetValue<string>("name", "Vending Machine");

        // Add roodCommand options
        var currencyOption = new Option<string>(
            name: "--currency",
            description: "currency accepted",
            getDefaultValue: () => defaultCurrency);

        var languageOption = new Option<string>(
            name: "--language",
            description: "selected language",
            getDefaultValue: () => defaultLanguage);

        // Create rootCommand
        var rootCommand = new RootCommand(name);
        rootCommand.AddOption(currencyOption);
        rootCommand.AddOption(languageOption);

        // Set rootCommand handler method
        rootCommand.SetHandler(async (language, currency) =>
            {
                var handler = new VendingMachineCommandHandler(this._svcProvider!);
                await handler.HandleAsync(language, currency, new CancellationTokenSource().Token);
            },
            languageOption, currencyOption);

        return rootCommand;
    }
}