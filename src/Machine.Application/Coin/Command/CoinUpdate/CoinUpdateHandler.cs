
using Machine.Application.Common.Interfaces;
using Machine.Application.Common.Exceptions;
using Serilog;
using Machine.Application.Coins.Commands.CoinUpdate;
using Machine.Application.Coins;

namespace Machine.Application.Coins.Commands.CoinUpdate;

public class CoinUpdateHandler : IRequestHandler<CoinUpdateCommand, CoinDto>
{
    private readonly IApplicationDbContext _dbContext;

    private readonly ILanguageService _lang;

    private readonly ILogger _logger = Log.ForContext<CoinUpdateHandler>();

    public CoinUpdateHandler(
        IApplicationDbContext dbContext,
        ILanguageService languageService
    )
    {
        _dbContext = dbContext;
        _lang = languageService;
    }

    public async Task<CoinDto> Handle(CoinUpdateCommand command, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Coin.FindAsync(new object?[] { command.CoinId }, cancellationToken);
        if (entity == null) throw new NotFoundException(_lang.Translate("ERR_DATA_NOT_FOUND"));


        entity.Count = command.Count;
        entity.CoinId = command.CoinId;


        _dbContext.Coin.Update(entity);
        int result = await _dbContext.SaveChangesAsync();
        _logger.Debug($"operation completed Command:{command}");

        return new CoinDto
        {
            Count = entity.Count,
            CoinId = entity.CoinId,
          
        };
    }
}
