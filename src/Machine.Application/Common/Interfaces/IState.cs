
using Machine.Application.Products;
using Machine.Domain.Entities;

namespace Machine.Application.Common.Interface;

public interface IState
{
    public List<ProductDto> Show();
    public Dictionary<decimal, int> Select(int productId);
    public void EnterCoin(decimal coin);
    public Dictionary<decimal, int> DispenseItem();
    public Dictionary<decimal, int> ReturnCoins();
}