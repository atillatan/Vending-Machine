using Machine.Application.Common.Interfaces;
using Serilog;
using Machine.Domain.Entities;

namespace Machine.Application.Languages.Commands.LanguageCreate;

public class LanguageCreateHandler : IRequestHandler<LanguageCreateCommand, LanguageDto>
{
    private readonly ILanguageService _lang;

    private readonly ILogger _logger = Log.ForContext<LanguageCreateHandler>();

    private readonly IApplicationDbContext _dBContext;

    public LanguageCreateHandler(ILanguageService languageService, IApplicationDbContext dbContext)
    {
        _lang = languageService;
        _dBContext = dbContext;
    }

    public async Task<LanguageDto> Handle(LanguageCreateCommand command, CancellationToken cancellationToken)
    {
        var entity = new Language
        {
            LanguageId = command.LanguageId,
            MessageKey = command.MessageKey,
            Message = command.Message
        };

        _dBContext.Language.Add(entity);
        int result = await _dBContext.SaveChangesAsync();
        _logger.Debug($"operation completed Command:{command}");
        return new LanguageDto().FromEntity(entity);
    }
}