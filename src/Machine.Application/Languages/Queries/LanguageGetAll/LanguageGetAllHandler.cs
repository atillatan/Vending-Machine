using Machine.Application.Common.Interfaces;
using Serilog;
using Microsoft.EntityFrameworkCore;
using Machine.Domain.Entities;

namespace Machine.Application.Languages.Queries.LanguageGetAll;

public class LanguageGetAllHandler : IRequestHandler<LanguageGetAllQuery, IEnumerable<LanguageDto>>
{
    private readonly IApplicationDbContext _dbContext;

    private readonly ILogger _logger = Log.ForContext<LanguageGetAllHandler>();

    public LanguageGetAllHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<LanguageDto>> Handle(LanguageGetAllQuery query, CancellationToken cancellationToken)
    {

        IEnumerable<Language>? result = await _dbContext.Set<Language>().ToListAsync(cancellationToken);

        _logger.Debug($"operation completed Command:{query}");
        return result.Select(entity => new LanguageDto().FromEntity(entity));
    }
}