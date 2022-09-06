using Machine.Application.Languages;

namespace Machine.Application.Languages.Commands.LanguageUpdate;

public class LanguageUpdateCommand : IRequest<LanguageDto>
{
    public string LanguageId { get; set; } = default!;
    public string MessageKey { get; set; } = default!;
    public string Message { get; set; } = default!;
}

public class LanguageUpdateCommandValidator : AbstractValidator<LanguageUpdateCommand>
{
    public LanguageUpdateCommandValidator()
    {
        RuleFor(m => m.LanguageId).NotNull().NotEmpty();
        RuleFor(m => m.MessageKey).NotNull().NotEmpty();
        RuleFor(m => m.Message).NotNull().NotEmpty();
    }
}
