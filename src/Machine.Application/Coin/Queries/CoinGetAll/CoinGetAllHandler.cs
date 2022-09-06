using Machine.Application.Common.Interfaces;
using Serilog;
using Microsoft.EntityFrameworkCore;
using Machine.Domain.Entities;
using Machine.Application.Coins;

namespace Machine.Application.Coins.Queries.CoinGetAll;

public class CoinGetAllHandler : IRequestHandler<CoinGetAllQuery, IEnumerable<CoinDto>>
{
    private readonly IApplicationDbContext _dbContext;

    private readonly ILogger _logger = Log.ForContext<CoinGetAllHandler>();

    public CoinGetAllHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<CoinDto>> Handle(CoinGetAllQuery query, CancellationToken cancellationToken)
    {

        IEnumerable<Coin>? result = await _dbContext.Set<Coin>().ToListAsync(cancellationToken);

        _logger.Debug($"operation completed Command:{query}");
        return result.Select(entity => CoinDto.FromEntity(entity));
    }
}