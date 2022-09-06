using Machine.Application.Common.Interfaces;
using Serilog;
using Machine.Domain.Entities;
using Machine.Application.Products;

namespace Machine.Application.Products.Commands.ProductCreate;

public class ProductCreateHandler : IRequestHandler<ProductCreateCommand, ProductDto>
{
    private readonly ILanguageService _lang;

    private readonly ILogger _logger = Log.ForContext<ProductCreateHandler>();

    private readonly IApplicationDbContext _dBContext;

    public ProductCreateHandler(ILanguageService languageService, IApplicationDbContext dbContext)
    {
        _lang = languageService;
        _dBContext = dbContext;
    }

    public async Task<ProductDto> Handle(ProductCreateCommand command, CancellationToken cancellationToken)
    {
        Product entity = new Product
        {
            SlotId = command.SlotId,
            ProductId = command.ProductId,
            ProductName = command.ProductName,           
            RemainingStock = command.RemainingStock,
            Capacity = command.Capacity
        };

        _dBContext.Product.Add(entity);
        int result = await _dBContext.SaveChangesAsync();
        _logger.Debug($"operation completed Command:{command}");
        return ProductDto.FromEntity(entity);
    }
}