using Machine.Domain.Entities;

namespace Machine.Application.Coins;

public class CoinDto
{
   
    public int Count { get; set; } = 0;
 
    public decimal CoinId {get;set;}

   
    public static CoinDto FromEntity(Coin entity)
    {
        return new CoinDto
        {
            Count = entity.Count,
            CoinId = entity.CoinId
        };
    }
    public Coin CopyToEntity(Coin entity)
    {
        entity.Count = this.Count;
        entity.CoinId = this.CoinId;        
        return entity;
    }
}
