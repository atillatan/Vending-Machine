namespace Machine.Application.Languages.Queries.LanguageFindByKey;

public class LanguageFindByKeyQuery : IRequest<LanguageDto?>
{
    public string LanguageId { get; set; } = default!;
    public string MessageKey { get; set; } = default!;
    public string Message { get; set; } = default!;
}
