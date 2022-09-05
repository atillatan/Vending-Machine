using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Machine.Application.Common.Interfaces;

public interface ILanguageService
{
    string DefaultLanguage { get; set; }

    string Translate(string key, string? language = null);

    ConcurrentDictionary<string, object> Cache(string key);

    ILanguageService UseLanguage(string lang);

}