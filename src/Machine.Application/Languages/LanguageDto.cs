using Machine.Domain.Entities;
namespace Machine.Application.Languages;

[Serializable]
public class LanguageDto
{
    public string LanguageId { get; set; } = default!;
    public string MessageKey { get; set; } = default!;
    public string Message { get; set; } = default!;

    public LanguageDto FromEntity(Language entity)
    {
        this.LanguageId = entity.LanguageId;
        this.MessageKey = entity.MessageKey;
        this.Message = entity.Message;
        return this;
    }
    public Language ToEntity(Language entity)
    {
        entity.LanguageId = this.LanguageId;
        entity.MessageKey = this.MessageKey;
        entity.Message = this.Message;
        return entity;
    }
}