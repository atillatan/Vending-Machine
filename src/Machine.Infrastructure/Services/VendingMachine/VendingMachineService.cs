using Machine.Application.Products;
using Machine.Application.Common.Exceptions;
using Machine.Application.Common.Interface;
using Machine.Domain.Entities;
using Machine.Infrastructure.Persistence;
using Machine.Infrastructure.Services.VendingMachine;
using Microsoft.Extensions.Configuration;
namespace Machine.Infrastructure.Services;

public class VendingMachineService : IVendingMachine
{
    public IState State { get; set; }

    public int? SelectedProduct { get; set; }

    public decimal CurrentBalance { get; set; }

    private string _currency { get; set; } = "EUR";

    private string _language { get; set; } = "EN";

    private Dictionary<decimal, int> _coinInventory { get; set; } = new Dictionary<decimal, int>();

    private List<Product> _productInventory { get; set; } = new List<Product>();

    private List<ProductPrice> _productPrices { get; set; } = new List<ProductPrice>();

    private IConfiguration _configuration { get; set; }

    private readonly ApplicationDbContext _dbContext;

    public VendingMachineService(IConfiguration configuration, ApplicationDbContext dbContext)
    {
        this._configuration = configuration;
        this._dbContext = dbContext;
        this.State = new ReadyState(this);

        this._coinInventory = this._dbContext.Coin.ToDictionary(x => x.CoinId, y => y.Count);
        this._productInventory = this._dbContext.Product.ToList();
        this._productPrices = this._dbContext.ProductPrice.ToList();
    }

    #region Vending Machine Operations
    public List<ProductDto> ProductList()
    {
        var queryable = from p in this._productInventory.AsEnumerable<Product>()
                        group p by p.ProductId into g
                        orderby g.Key
                        select new ProductDto
                        {
                            ProductId = g.Key,
                            ProductName = g.Select(a => a.ProductName).FirstOrDefault()!,
                            ProductPrice = this._productPrices!.Where(b => b.ProductId == g.Key && b.CurrencyId == this._currency).FirstOrDefault<ProductPrice>()!.Price,
                            RemainingStock = g.Sum(b => b.RemainingStock),
                            Capacity = g.Sum(c => c.Capacity)
                        };

        List<ProductDto> result = queryable.ToList<ProductDto>();

        return result;
    }

    public decimal? SelectedProductPrice()
    {
        if (this.SelectedProduct == null) return null;

        var productPrice = this._productPrices
                                .Where(p => p.ProductId == this.SelectedProduct && p.CurrencyId == this._currency)
                                .FirstOrDefault();

        return productPrice != null ? productPrice.Price : null;
    }

    public void AddCoin(decimal coin)
    {
        // Validate if accepted coin type
        if (!_coinInventory.ContainsKey(coin)) throw new InvalidCoinException();

        // Update database
        var coinEntity = _dbContext.Coin.Where(x => x.CoinId == coin).FirstOrDefault();
        if (coinEntity != null)
        {
            coinEntity.Count += 1;
            _dbContext.Coin.Update(coinEntity);
            _dbContext.SaveChanges();

            // Add the coin to the current balance
            this.CurrentBalance += coin;
        }

        // Reload data
        this._coinInventory = this._dbContext.Coin.ToDictionary(x => x.CoinId, y => y.Count);
    }

    public Dictionary<decimal, int> ReturnCustomerCoins()
    {
        if (!IsChangePossible(this.CurrentBalance)) throw new ExactChangeOnlyException();

        return MakeChange(0.00m);
    }

    public void DispenseSelectedProduct()
    {
        foreach (var product in this._productInventory)
        {
            if (product.ProductId == this.SelectedProduct && product.RemainingStock > 0)
            {
                // Update database
                var productEntity = _dbContext.Product.Where(x => x.ProductId == product.ProductId && x.SlotId == product.SlotId).FirstOrDefault();

                if (productEntity != null)
                {
                    productEntity.RemainingStock -= 1;
                    _dbContext.Product.Update(productEntity);
                    // product.RemainingStock -= 1;
                    this._productInventory = this._dbContext.Product.ToList();
                }

                break;
            }
        }
    }

    public Dictionary<decimal, int> MakeChange(decimal itemPrice)
    {
        decimal changeToPay = this.CurrentBalance;

        if (this.SelectedProductPrice() != null) changeToPay = this.CurrentBalance - (decimal)this.SelectedProductPrice()!;

        var change = new Dictionary<decimal, int>();

        // Dispense coins
        foreach (var coin in this._coinInventory)
        {
            int count = (int)(changeToPay / coin.Key);

            for (int i = 0; i < count; i++)
            {
                if (this._coinInventory[coin.Key] > 0)
                {
                    var coinEntity = _dbContext.Coin.Where(x => x.CoinId == coin.Key).FirstOrDefault();

                    if (coinEntity != null)
                    {
                        //this._coinInventory[coin.Key] -= 1;
                        coinEntity.Count -= 1;
                        _dbContext.Coin.Update(coinEntity);
                    }

                    // Collect change
                    changeToPay -= coin.Key;
                    if (!change.ContainsKey(coin.Key)) change.Add(coin.Key, 0);
                    change[coin.Key] += 1;
                }
                else
                {
                    break;
                }
            }
        }
        this.CurrentBalance = 0;

        // Update database     
        _dbContext.SaveChanges();
        this._coinInventory = this._dbContext.Coin.ToDictionary(x => x.CoinId, y => y.Count);
        return change;
    }

    public bool IsChangePossible(decimal balance)
    {
        var change = new Dictionary<decimal, int>();

        // Clone variables      
        decimal tempBalance = balance;
        Dictionary<decimal, int> tempCoinInventory = this._coinInventory.ToDictionary(x => x.Key, x => x.Value);

        foreach (var coin in tempCoinInventory)
        {
            int count = (int)(tempBalance / coin.Key);

            for (int i = 0; i < count; i++)
            {
                if (tempCoinInventory[coin.Key] > 0)
                {
                    tempCoinInventory[coin.Key] -= 1;
                    tempBalance -= coin.Key;
                    if (!change.ContainsKey(coin.Key)) change.Add(coin.Key, 0);
                    change[coin.Key] += 1;
                }
                else
                {
                    break;
                }
            }
        }

        decimal changeBalance = change.Sum(x => Convert.ToDecimal(x.Key) * x.Value);

        return balance == changeBalance;
    }

    public bool IsValidCoin(decimal coin)
    {
        return _coinInventory.ContainsKey(coin);
    }
    public bool IsValidProduct(int productId)
    {
        return this._productPrices
                .Where(p => p.ProductId == productId && p.CurrencyId == this._currency)
                .Any();
    }

    /// <summary>
    /// Fluent interface for setting language
    /// </summary>
    /// <param name="languageCode"></param>
    /// <returns></returns>
    public IVendingMachine UseLanguage(string languageCode)
    {
        this._language = languageCode;
        return this;
    }

    /// <summary>
    /// Fluent interface for setting currency
    /// </summary>
    /// <param name="currency"></param>
    /// <returns></returns>
    public IVendingMachine UseCurrency(string currency)
    {
        this._currency = currency;
        return this;
    }
    #endregion

    #region IState Operations, Customer Actions

    // State specific operations will be transferred to the corresponding state
    public List<ProductDto> Show() => this.State.Show();

    public Dictionary<decimal, int> Select(int productId) => this.State.Select(productId);

    public void EnterCoin(decimal coin) => this.State.EnterCoin(coin);

    public void DispenseItem() => this.State.DispenseItem();

    public Dictionary<decimal, int> ReturnCoins() => this.State.ReturnCoins();

    #endregion
}

