using Machine.Application.Common.Interfaces;
using Serilog;
using Machine.Application.Common.Exceptions;

namespace Machine.Application.Languages.Queries.LanguageFindByKey;

public class LanguageFindByKeyHandler : IRequestHandler<LanguageFindByKeyQuery, LanguageDto?>
{
    private readonly IApplicationDbContext _dbContext;

    private readonly ILanguageService _lang; 

    private readonly ILogger _logger = Log.ForContext<LanguageFindByKeyHandler>();

    public LanguageFindByKeyHandler(
        IApplicationDbContext dbContext,
        ILanguageService languageService
    )
    {
        _dbContext = dbContext;
        _lang = languageService;
    }

    public async Task<LanguageDto?> Handle(LanguageFindByKeyQuery query, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Language.FindAsync(new string[] { query.LanguageId, query.MessageKey }, cancellationToken);
        if (entity == null) throw new NotFoundException(_lang.Translate("ERR_DATA_NOT_FOUND"));

        _logger.Debug($"operation completed Command:{query}");
        return new LanguageDto().FromEntity(entity);
    }
}