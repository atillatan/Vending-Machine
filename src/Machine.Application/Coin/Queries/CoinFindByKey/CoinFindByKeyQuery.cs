using Machine.Application.Coins;

namespace Machine.Application.Coins.Queries.CoinFindByKey;

public class CoinFindByKeyQuery : IRequest<CoinDto?>
{
    public decimal CoinId { get; set; } = default!;
     
}
