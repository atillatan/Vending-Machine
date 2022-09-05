using Machine.Application.Common.Dto;
using Machine.Domain.Entities;
namespace Machine.Application.Common.Interface;

public interface IVendingMachine
{
    IState State { get; set; }

    int? SelectedProduct { get; set; }

    decimal CurrentBalance { get; set; }

    List<ProductDto> ProductList();

    void AddCoin(decimal coin);

    Dictionary<decimal, int> ReturnCustomerCoins();

    decimal? SelectedProductPrice();

    void DispenseSelectedProduct();

    Dictionary<decimal, int> MakeChange(decimal itemPrice);

    bool IsChangePossible(decimal balance);

    bool IsValidCoin(decimal coin);

    bool IsValidProduct(int productId);

    IVendingMachine UseLanguage(string languageCode);

    IVendingMachine UseCurrency(string currency);

    List<ProductDto> Show();

     Dictionary<decimal, int> Select(int productId);

    void EnterCoin(decimal coin);

    void DispenseItem();

    Dictionary<decimal, int> ReturnCoins();
}