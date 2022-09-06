using Machine.Application.Products;
using Machine.Application.Common.Exceptions;
using Machine.Application.Common.Interface;

namespace Machine.Infrastructure.Services.VendingMachine;

public class DispenseItemState : IState
{
    public IVendingMachine _vendingMachine { get; set; }
    public DispenseItemState(IVendingMachine vendingMachine)
    {
        this._vendingMachine = vendingMachine;
    }
    public List<ProductDto> Show()
    {
        // Stay same state, keep current balance
        return this._vendingMachine.ProductList();
    }
    public  Dictionary<decimal, int> Select(int productId)
    {
        throw new InvalidStateException();
    }

    public void EnterCoin(decimal coin)
    {
        throw new InvalidStateException();
    }

    public Dictionary<decimal, int> DispenseItem()
    {       
        this._vendingMachine.DispenseSelectedProduct();    
        var payToChange = this._vendingMachine.MakeChange((decimal)this._vendingMachine.SelectedProductPrice()!);           
        this._vendingMachine.State = new ReadyState(this._vendingMachine);
        return payToChange;
    }

    public Dictionary<decimal, int> ReturnCoins()
    {
        throw new InvalidStateException();
    }
}