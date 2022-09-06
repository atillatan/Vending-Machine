using System.Collections.Generic;
using Machine.Application.Products;
using Machine.Application.Common.Interface;
using Machine.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
public class VendingMachineCommandHandler
{
    private IServiceProvider? _svcProvider { get; set; }

    private IConfiguration? _configuration { get; set; }

    private ILanguageService _langSvc { get; set; } = default!;

    public VendingMachineCommandHandler(IServiceProvider serviceProvider)
    {
        this._svcProvider = serviceProvider;
        this._configuration = _svcProvider!.GetRequiredService<IConfiguration>();
    }

    // RootCommand handler method
    public async Task HandleAsync(string language, string currency, CancellationToken cancellationToken)
    {
        Console.Title = _configuration.GetValue<string>("name","Vending Machine");
        Console.WriteLine("***********************************************************");
        Console.WriteLine("******************* WELCOME TO VENDING MACHINE  ***********");
        Console.WriteLine("***********************************************************");
        Console.WriteLine("                                                           ");

        try
        {
            // Load required configuration
            string[] languages = _configuration!.GetSection("languages").Get<string[]>();
            string[] currencies = _configuration!.GetSection("currencies").Get<string[]>();
            string defaultLanguage = _configuration.GetValue<string>("default.language", "EN");
            string defaultCurrency = _configuration.GetValue<string>("default.currency", "EUR");

            // override the default configuration with command parameters
            if (languages.Contains(language)) defaultLanguage = language;
            if (currencies.Contains(currency)) defaultCurrency = currency;

            var vendingMachine = _svcProvider!.GetRequiredService<IVendingMachine>()
                            .UseLanguage(defaultLanguage)
                            .UseCurrency(defaultCurrency);

            this._langSvc = _svcProvider!.GetRequiredService<ILanguageService>()
                             .UseLanguage(defaultLanguage);


            Console.WriteLine($"Please \"{_langSvc.Translate("INSERT_COIN")}\", for help please enter \"HELP\" ");

            // Loop until shutdown
            bool isRunning = true;

            while (isRunning)
            {
                string userInput = await WaitValidUserInputAsync(cancellationToken);
                string enteredCommand = GetEnteredCommand(userInput);
                try
                {
                    switch (enteredCommand)
                    {
                        case "SHOW": // READY, ACCEPTCOIN
                            Console.WriteLine(" ");
                            Console.WriteLine("Product List:");
                            Console.WriteLine(" ");
                            PrintProducts(vendingMachine.Show(), defaultCurrency);

                            if (vendingMachine.CurrentBalance > 0)
                                Console.WriteLine($"{_langSvc.Translate("AMOUNT_ENTERED")}: {vendingMachine.CurrentBalance} {defaultCurrency}");
                            else
                                Console.WriteLine(_langSvc.Translate("INSERT_COIN"));
                            break;

                        case "ENTER": // READY, ACCEPTCOIN
                            decimal enteredCoin = GetDecimalParamFromCommand(userInput);
                            vendingMachine.EnterCoin(enteredCoin);

                            if (vendingMachine.CurrentBalance > 0)
                                Console.WriteLine($"{_langSvc.Translate("AMOUNT_ENTERED")}: {vendingMachine.CurrentBalance} {defaultCurrency}");
                            else
                                Console.WriteLine(_langSvc.Translate("INSERT_COIN"));
                            break;

                        case "SELECT": // ACCEPTCOIN
                            int selectedProduct = GetIntParamFromCommand(userInput);
                            Console.WriteLine($"{_langSvc.Translate("SELECTED_PRODUCT")}: {selectedProduct}");
                            var coins = vendingMachine.Select(selectedProduct);
                            decimal sumCoin = coins.Sum(x => x.Key * x.Value);

                            if (sumCoin > 0)
                                Console.WriteLine($"{_langSvc.Translate("PLEASE_TAKE_YOUR_CHANGE")}: {sumCoin} {defaultCurrency},{_langSvc.Translate("THANK_YOU")}!");
                            else
                                Console.WriteLine(_langSvc.Translate("THANK_YOU"));
                            break;

                        case "RETURNCOINS": // ACCEPTCOIN
                            var returnedCoins = vendingMachine.ReturnCoins();
                            decimal sumReturnedCoins = returnedCoins.Sum(x => x.Key * x.Value);

                            if (sumReturnedCoins > 0)
                                Console.WriteLine($"{_langSvc.Translate("PLEASE_TAKE_YOUR_CHANGE")}: {sumReturnedCoins} {defaultCurrency}");
                            else
                                Console.WriteLine(_langSvc.Translate("INSERT_COIN"));
                            break;

                        case "HELP":
                            PrintHelp();
                            break;

                        case "UNKNOWN":
                            await Console.Error.WriteLineAsync(_langSvc.Translate("PLEASE_PROVIDE_VALID_COMMAND"));
                            break;

                        case "EXIT":
                            isRunning = false;
                            break;

                        default:
                            if (enteredCommand.Length < 3)
                                Console.WriteLine("Please enter command first e.g. COMMAND <XXX>.  ENTER 0.65");
                            else
                                Console.Error.WriteLine(_langSvc.Translate("PLEASE_PROVIDE_VALID_COMMAND"));
                            break;
                    }

                }
                catch (System.Exception ex)
                {
                    Console.Error.WriteLine((ex.Message != null ? ex.Message.ToString() : ex));
                }
            }
        }
        catch (OperationCanceledException)
        {
            Console.Error.WriteLine("Vending Machine operation cancelled!");
        }
    }

    #region Helper Methods

    // Recursive function for waiting valid user input
    private async Task<string> WaitValidUserInputAsync(CancellationToken cancellationToken)
    {
        var input = Console.ReadLine();

        if (string.IsNullOrEmpty(input))
        {
            await Console.Error.WriteLineAsync(_langSvc.Translate("PLEASE_PROVIDE_VALID_COMMAND"));
            return await WaitValidUserInputAsync(cancellationToken);
        }

        input = input!.Trim().ToUpper();

        if (input.StartsWith("ENTER")
            || input.StartsWith("SELECT")
            || input.Equals("SHOW")
            || input.Equals("RETURNCOINS")
            || input.Equals("HELP")
            || input.Equals("EXIT"))
        {
            return input;
        }

        Console.Error.WriteLine(_langSvc.Translate("PLEASE_PROVIDE_VALID_COMMAND"));
        return await WaitValidUserInputAsync(cancellationToken);
    }

    private string GetEnteredCommand(string input)
    {
        input = input!.Trim().ToUpper();

        if (input.StartsWith("SHOW"))
            return "SHOW";
        else if (input.StartsWith("SELECT"))
            return "SELECT";
        else if (input.StartsWith("ENTER"))
            return "ENTER";
        else if (input.StartsWith("RETURNCOINS"))
            return "RETURNCOINS";
        else if (input.StartsWith("HELP"))
            return "HELP";
        else if (input.StartsWith("EXIT"))
            return "EXIT";
        else
            return "UNKNOWN";
    }

    private void PrintHelp()
    {
        Console.WriteLine("");
        Console.WriteLine("Description:  ");
        Console.WriteLine("  Vending Machine");
        Console.WriteLine("");
        Console.WriteLine("Commands:");
        Console.WriteLine("  SHOW        : Then list of products. e.g SHOW");
        Console.WriteLine("  SELECT      : Choose which product do you want to buy. e.g SELECT <NUMBER> , SELECT 2");
        Console.WriteLine("  ENTER       : Enter valid coins (5cts to 2€). ENTER <XXX> e.g. : ENTER 0.65");
        Console.WriteLine("  RETURNCOINS : the money the customer has placed in the machine is returned indicating returned money");
        Console.WriteLine("  EXIT        : Exit from Vending Machine");
    }

    private void PrintProducts(List<ProductDto> productList, string currency)
    {
        foreach (ProductDto product in productList)
        {
            Console.WriteLine($"{product.ProductId}. {product.ProductName} -  {product.ProductPrice} {currency} - {product.RemainingStock} Items Left");
        }
    }

    private decimal GetDecimalParamFromCommand(string command)
    {
        if (!command.Contains(" ") || command.Split(' ')[1] == null)
            throw new ArgumentException(_langSvc.Translate("PLEASE_PROVIDE_VALID_COMMAND"));

        decimal result;
        if (!decimal.TryParse(command.Split(' ')[1], out result))
            throw new ArgumentException(_langSvc.Translate("PLEASE_PROVIDE_VALID_COMMAND"));

        return result;
    }
    private int GetIntParamFromCommand(string command)
    {
        if (!command.Contains(" ") || command.Split(' ')[1] == null)
            throw new ArgumentException(_langSvc.Translate("PLEASE_PROVIDE_VALID_COMMAND"));

        int result;
        if (!int.TryParse(command.Split(' ')[1], out result))
            throw new ArgumentException(_langSvc.Translate("PLEASE_PROVIDE_VALID_COMMAND"));

        return result;
    }
    #endregion

}