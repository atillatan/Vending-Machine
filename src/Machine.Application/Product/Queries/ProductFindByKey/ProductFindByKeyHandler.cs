using Machine.Application.Common.Interfaces;
using Serilog;
using Machine.Application.Common.Exceptions;
using Machine.Application.Products;

namespace Machine.Application.Products.Queries.ProductFindByKey;

public class ProductFindByKeyHandler : IRequestHandler<ProductFindByKeyQuery, ProductDto?>
{
    private readonly IApplicationDbContext _dbContext;

    private readonly ILanguageService _lang; 

    private readonly ILogger _logger = Log.ForContext<ProductFindByKeyHandler>();

    public ProductFindByKeyHandler(
        IApplicationDbContext dbContext,
        ILanguageService languageService
    )
    {
        _dbContext = dbContext;
        _lang = languageService;
    }

    public async Task<ProductDto?> Handle(ProductFindByKeyQuery query, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Product.FindAsync(new string[] { query.ProductId.ToString(), query.SlotId }, cancellationToken);
        if (entity == null) throw new NotFoundException(_lang.Translate("ERR_DATA_NOT_FOUND"));

        _logger.Debug($"operation completed Command:{query}");
        return ProductDto.FromEntity(entity);
    }
}