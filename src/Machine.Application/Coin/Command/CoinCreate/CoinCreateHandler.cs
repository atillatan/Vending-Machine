using Machine.Application.Common.Interfaces;
using Serilog;
using Machine.Domain.Entities;
using Machine.Application.Coins;

namespace Machine.Application.Coins.Commands.CoinCreate;

public class CoinCreateHandler : IRequestHandler<CoinCreateCommand, CoinDto>
{
    private readonly ILanguageService _lang;

    private readonly ILogger _logger = Log.ForContext<CoinCreateHandler>();

    private readonly IApplicationDbContext _dBContext;

    public CoinCreateHandler(ILanguageService languageService, IApplicationDbContext dbContext)
    {
        _lang = languageService;
        _dBContext = dbContext;
    }

    public async Task<CoinDto> Handle(CoinCreateCommand command, CancellationToken cancellationToken)
    {
        Coin entity = new Coin
        {   
            CoinId = command.CoinId,
            Count = command.Count           
        };

        _dBContext.Coin.Add(entity);
        int result = await _dBContext.SaveChangesAsync();
        _logger.Debug($"operation completed Command:{command}");
        return CoinDto.FromEntity(entity);
    }
}