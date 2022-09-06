namespace Machine.Application.Languages.Commands.LanguageDelete;

public class LanguageDeleteCommand : IRequest<bool>
{
    public string LanguageId { get; set; } = default!;
    public string MessageKey { get; set; } = default!;
}

public class LanguageDeleteCommandValidator : AbstractValidator<LanguageDeleteCommand>
{
    public LanguageDeleteCommandValidator()
    {
        RuleFor(m => m.LanguageId).NotEmpty();
        RuleFor(m => m.MessageKey).NotEmpty();
    }
}
