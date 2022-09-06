using Machine.Application.Common.Interfaces;
using Serilog;
using Machine.Application.Common.Exceptions;
using Machine.Application.Coins;

namespace Machine.Application.Coins.Queries.CoinFindByKey;

public class CoinFindByKeyHandler : IRequestHandler<CoinFindByKeyQuery, CoinDto?>
{
    private readonly IApplicationDbContext _dbContext;

    private readonly ILanguageService _lang; 

    private readonly ILogger _logger = Log.ForContext<CoinFindByKeyHandler>();

    public CoinFindByKeyHandler(
        IApplicationDbContext dbContext,
        ILanguageService languageService
    )
    {
        _dbContext = dbContext;
        _lang = languageService;
    }

    public async Task<CoinDto?> Handle(CoinFindByKeyQuery query, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Coin.FindAsync(new object?[] { query.CoinId }, cancellationToken);
        if (entity == null) throw new NotFoundException(_lang.Translate("ERR_DATA_NOT_FOUND"));

        _logger.Debug($"operation completed Command:{query}");
        return CoinDto.FromEntity(entity);
    }
}