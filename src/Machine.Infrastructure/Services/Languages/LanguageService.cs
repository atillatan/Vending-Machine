using Machine.Application.Common.Interfaces;
using System.Collections.Concurrent;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Machine.Infrastructure.Persistence;
using Machine.Domain.Entities;

namespace Machine.Infrastructure.Services.Languages;

public class LanguageService : ILanguageService
{

    private readonly IConfiguration _configuration;
    private readonly IApplicationDbContext _applicationDbContext;

    public string DefaultLanguage { get; set; }

    public LanguageService(IConfiguration configuration, IApplicationDbContext applicationDbContext)
    {
        this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        this._applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
        this.DefaultLanguage = _configuration.GetValue<string>("default.language");
        LoadLanguageTable();
    }

    private void LoadLanguageTable()
    {
        ApplicationDbContext? svc = this._applicationDbContext as ApplicationDbContext;

        if (svc != null && svc.Database.CanConnect() && !_cacheTable.Any())
        {
            IQueryable<Language> q = svc.Language.AsNoTracking();
            var entityList = q.ToList();

            foreach (Language lang in entityList!)
            {
                object _value = lang.Message;
                if (!this.Cache(lang.LanguageId.ToUpper()).TryGetValue(lang.MessageKey, out _value!))
                    this.Cache(lang.LanguageId.ToUpper())[lang.MessageKey] = lang.Message;
            }
        }
    }

    public string Translate(string key, string? language = null)
    {
        if (string.IsNullOrEmpty(language)) language = DefaultLanguage;
        if (language.Length > 2) language = language.Substring(0, 2);

        object langValue = key;
        Cache(language.ToUpper()).TryGetValue(key, out langValue!);
        if (langValue != null)
            return (string)langValue;
        else
        {
            // Cache(language).Clear();
            // LoadLanguageTable();
            return key as string;
        }
    }

    public ILanguageService UseLanguage(string lang)
    {
        this.DefaultLanguage = lang;
        return this;
    }

    private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, object>> _cacheTable = new ConcurrentDictionary<string, ConcurrentDictionary<string, object>>();

    public ConcurrentDictionary<string, object> Cache(string key)
    {
        ConcurrentDictionary<string, object> internalCache;
        if (_cacheTable.TryGetValue(key, out internalCache!)) return internalCache;

        ConcurrentDictionary<string, object> newCache = new ConcurrentDictionary<string, object>();

        _cacheTable[key] = newCache;

        return newCache;
    }
}