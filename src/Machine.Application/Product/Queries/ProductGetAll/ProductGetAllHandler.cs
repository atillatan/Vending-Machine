using Machine.Application.Common.Interfaces;
using Serilog;
using Microsoft.EntityFrameworkCore;
using Machine.Domain.Entities;
using Machine.Application.Products;

namespace Machine.Application.Products.Queries.ProductGetAll;

public class ProductGetAllHandler : IRequestHandler<ProductGetAllQuery, IEnumerable<ProductDto>>
{
    private readonly IApplicationDbContext _dbContext;

    private readonly ILogger _logger = Log.ForContext<ProductGetAllHandler>();

    public ProductGetAllHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<ProductDto>> Handle(ProductGetAllQuery query, CancellationToken cancellationToken)
    {

        IEnumerable<Product>? result = await _dbContext.Set<Product>().ToListAsync(cancellationToken);

        _logger.Debug($"operation completed Command:{query}");
        return result.Select(entity => ProductDto.FromEntity(entity));
    }
}