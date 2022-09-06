using Machine.Domain.Entities;

namespace Machine.Application.Products;

public class ProductDto
{
    public string SlotId { get; set; } = default!;

    public int ProductId { get; set; } = 0;

    public string ProductName { get; set; } = default!;

    public decimal? ProductPrice {get;set;}

    public int RemainingStock { get; set; } = 0;

    public int Capacity { get; set; } = 0;

    public static ProductDto FromEntity(Product entity)
    {
        return new ProductDto
        {
            SlotId = entity.SlotId,
            ProductId = entity.ProductId,
            ProductName = entity.ProductName,
            ProductPrice = null,
            RemainingStock = entity.RemainingStock,
            Capacity = entity.Capacity
        };
    }
    public Product CopyToEntity(Product entity)
    {
        entity.SlotId = this.SlotId;
        entity.ProductId = this.ProductId;
        entity.ProductName = this.ProductName;        
        entity.RemainingStock = this.RemainingStock;
        entity.Capacity = this.Capacity;
        return entity;
    }
}
