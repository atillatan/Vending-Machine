
using Machine.Application.Common.Interfaces;
using Machine.Application.Common.Exceptions;
using Serilog;

namespace Machine.Application.Languages.Commands.LanguageUpdate;

public class LanguageUpdateHandler : IRequestHandler<LanguageUpdateCommand, LanguageDto>
{
    private readonly IApplicationDbContext _dbContext;

    private readonly ILanguageService _lang;

    private readonly ILogger _logger = Log.ForContext<LanguageUpdateHandler>();

    public LanguageUpdateHandler(
        IApplicationDbContext dbContext,
        ILanguageService languageService
    )
    {
        _dbContext = dbContext;
        _lang = languageService;
    }

    public async Task<LanguageDto> Handle(LanguageUpdateCommand command, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Language.FindAsync(new string[] { command.LanguageId, command.MessageKey }, cancellationToken);
        if (entity == null) throw new NotFoundException(_lang.Translate("ERR_DATA_NOT_FOUND"));

        entity.LanguageId = command.LanguageId;
        entity.MessageKey = command.MessageKey;
        entity.Message = command.Message;

        _dbContext.Language.Update(entity);
        int result = await _dbContext.SaveChangesAsync();
        _logger.Debug($"operation completed Command:{command}");

        return new LanguageDto
        {
            LanguageId = entity.LanguageId,
            MessageKey = entity.MessageKey,
            Message = entity.Message
        };
    }
}
