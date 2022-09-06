using Machine.Application.Products;
using Machine.Application.Common.Exceptions;
using Machine.Application.Common.Interface;

namespace Machine.Infrastructure.Services.VendingMachine;

public class ReadyState : IState
{
    public IVendingMachine _vendingMachine { get; set; }

    public ReadyState(IVendingMachine vendingMachine)
    {
        this._vendingMachine = vendingMachine;
        this._vendingMachine.SelectedProduct = null;
    }
    public List<ProductDto> Show()
    {
        // Stay same state, keep current balance
        return this._vendingMachine.ProductList();
    }

    public void EnterCoin(decimal coin)
    {
        // Validate coin
        if (!this._vendingMachine.IsValidCoin(coin)) throw new InvalidCoinException();

        // Add coin to current balance,  
        this._vendingMachine.AddCoin(coin);

        // Change state to ACCEPT COIN  state        
        this._vendingMachine.State = new AcceptCoinState(this._vendingMachine);
    }

    public  Dictionary<decimal, int> Select(int productId)
    {
        throw new InvalidStateException("Please INSERT COIN!");
    }

    public Dictionary<decimal, int> DispenseItem()
    {
        // VendingMachine cannot sell item in this state
        // Product not selected and empty balance in this state
        throw new InvalidStateException("Please INSERT COIN!");
    }

    public Dictionary<decimal, int> ReturnCoins()
    {
        // Return customer coins        
        Dictionary<decimal, int> result = _vendingMachine.ReturnCustomerCoins();

        // Change state to READY state
        _vendingMachine.State = new ReadyState(this._vendingMachine);

        return result;
    }
}