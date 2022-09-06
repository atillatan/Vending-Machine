
using Machine.Application.Common.Interfaces;
using Machine.Application.Common.Exceptions;
using Serilog;
using Machine.Application.Products.Commands.ProductUpdate;
using Machine.Application.Products;

namespace Machine.Application.Products.Commands.ProductUpdate;

public class ProductUpdateHandler : IRequestHandler<ProductUpdateCommand, ProductDto>
{
    private readonly IApplicationDbContext _dbContext;

    private readonly ILanguageService _lang;

    private readonly ILogger _logger = Log.ForContext<ProductUpdateHandler>();

    public ProductUpdateHandler(
        IApplicationDbContext dbContext,
        ILanguageService languageService
    )
    {
        _dbContext = dbContext;
        _lang = languageService;
    }

    public async Task<ProductDto> Handle(ProductUpdateCommand command, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Product.FindAsync(new string[] { command.ProductId.ToString(), command.SlotId }, cancellationToken);
        if (entity == null) throw new NotFoundException(_lang.Translate("ERR_DATA_NOT_FOUND"));


        entity.SlotId = command.SlotId;
        entity.ProductId = command.ProductId;
        entity.ProductName = command.ProductName;
        entity.RemainingStock = command.RemainingStock;
        entity.Capacity = command.Capacity;

        _dbContext.Product.Update(entity);
        int result = await _dbContext.SaveChangesAsync();
        _logger.Debug($"operation completed Command:{command}");

        return new ProductDto
        {
            SlotId = entity.SlotId,
            ProductId = entity.ProductId,
            ProductName = entity.ProductName,
            RemainingStock = entity.RemainingStock,
            Capacity = entity.Capacity
        };
    }
}
