using Machine.Application.Common.Interfaces;
using Machine.Application.Common.Exceptions;
using Serilog;
namespace Machine.Application.Coins.Commands.CoinDelete;

public class CoinDeleteHandler : IRequestHandler<CoinDeleteCommand, bool>
{
    private readonly ILanguageService _lang;

    private readonly ILogger _logger = Log.ForContext<CoinDeleteHandler>();

    private readonly IApplicationDbContext _dBContext;

    public CoinDeleteHandler(ILanguageService languageService, IApplicationDbContext dbContext)
    {
        _dBContext = dbContext;
        _lang = languageService;
    }

    public async Task<bool> Handle(CoinDeleteCommand command, CancellationToken cancellationToken)
    {
        var entity = await _dBContext.Coin.FindAsync(new object?[] {command.CoinId }, cancellationToken);
        if (entity == null) throw new NotFoundException(_lang.Translate("ERR_DATA_NOT_FOUND"));

        _dBContext.Coin.Remove(entity);
        int result = await _dBContext.SaveChangesAsync(cancellationToken);
        _logger.Debug($"operation completed Command:{command}");
        return result > 0;
    }
}
