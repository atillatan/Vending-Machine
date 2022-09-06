using Machine.Application.Products;
using Machine.Application.Common.Exceptions;
using Machine.Application.Common.Interface;

namespace Machine.Infrastructure.Services.VendingMachine;

public class AcceptCoinState : IState
{

    public IVendingMachine _vendingMachine { get; set; }

    public AcceptCoinState(IVendingMachine vendingMachine)
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
        // Validate productId
        if (!this._vendingMachine.IsValidProduct(productId)) throw new InvalidProductException();

        // set selected product in current transaction
        _vendingMachine.SelectedProduct = productId;

        // Check current balance against selected product
        if (this._vendingMachine.CurrentBalance >= this._vendingMachine.SelectedProductPrice())
        { 
            decimal changeToPay = this._vendingMachine.CurrentBalance - (decimal)this._vendingMachine.SelectedProductPrice()!;

            if (!this._vendingMachine.IsChangePossible(changeToPay))
            {
                throw new ExactChangeOnlyException();
            }            
            this._vendingMachine.State = new DispenseItemState(this._vendingMachine);
            return this._vendingMachine.State.DispenseItem();
        }
        else
        {
            // if current balance not enough to buy this product
            throw new InvalidStateException("PRICE");
        }
    }

    public void EnterCoin(decimal coin)
    {
        // Validate coin
        if (!this._vendingMachine.IsValidCoin(coin)) throw new InvalidCoinException();

        // Add coin to current balance,  
        this._vendingMachine.AddCoin(coin);

        // stay same state
    }

    public Dictionary<decimal, int> DispenseItem()
    {
        // VendingMachine cannot sell item in this state
        // Product not selected and empty balance in this state
        throw new InvalidStateException();
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