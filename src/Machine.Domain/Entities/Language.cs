using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Machine.Domain.Entities;


[Table("LANGUAGE")]
[Serializable]
public class Language
{
    [Key]
    public string LanguageId { get; set; } = default!;

    [Key]
    public string MessageKey { get; set; } = default!;

    [Required]
    public string Message { get; set; } = default!;

}
