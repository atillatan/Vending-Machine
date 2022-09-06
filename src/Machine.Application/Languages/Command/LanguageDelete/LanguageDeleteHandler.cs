using Machine.Application.Common.Interfaces;
using Machine.Application.Common.Exceptions;
using Serilog;
namespace Machine.Application.Languages.Commands.LanguageDelete;

public class LanguageDeleteHandler : IRequestHandler<LanguageDeleteCommand, bool>
{
    private readonly ILanguageService _lang;

    private readonly ILogger _logger = Log.ForContext<LanguageDeleteHandler>();

    private readonly IApplicationDbContext _dBContext;

    public LanguageDeleteHandler(ILanguageService languageService, IApplicationDbContext dbContext)
    {
        _dBContext = dbContext;
        _lang = languageService;
    }

    public async Task<bool> Handle(LanguageDeleteCommand command, CancellationToken cancellationToken)
    {
        var entity = await _dBContext.Language.FindAsync(new string[] { command.LanguageId, command.MessageKey }, cancellationToken);
        if (entity == null) throw new NotFoundException(_lang.Translate("ERR_DATA_NOT_FOUND"));

        _dBContext.Language.Remove(entity);
        int result = await _dBContext.SaveChangesAsync(cancellationToken);
        _logger.Debug($"operation completed Command:{command}");
        return result > 0;
    }
}
