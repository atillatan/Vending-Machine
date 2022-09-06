using FluentValidation;
using Machine.Application.Languages;

namespace Machine.Application.Languages.Commands.LanguageCreate;

public class LanguageCreateCommand : IRequest<LanguageDto>
{
    public string LanguageId { get; set; } = default!;
    public string MessageKey { get; set; } = default!;
    public string Message { get; set; } = default!;
}

public class LanguageCreateCommandValidator : AbstractValidator<LanguageCreateCommand>
{
    public LanguageCreateCommandValidator()
    {
        RuleFor(m => m.LanguageId).NotNull().NotEmpty();
        RuleFor(m => m.MessageKey).NotNull().NotEmpty();
        RuleFor(m => m.Message).NotNull().NotEmpty();
    }
}