using Machine.Application.Products;

namespace Machine.Application.Products.Queries.ProductFindByKey;

public class ProductFindByKeyQuery : IRequest<ProductDto?>
{
    public int ProductId { get; set; } = default!;
    public string SlotId { get; set; } = default!;    
}
