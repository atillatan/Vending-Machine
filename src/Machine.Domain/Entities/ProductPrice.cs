

namespace Machine.Domain.Entities;

[Table("PRODUCT_PRICE")]
public class ProductPrice
{
    [Key]
    public int ProductId { get; set; } = default!;

    [Key]
    public string CurrencyId { get; set; } = default!;

    [Column(TypeName = "decimal(3,2)")]
    public decimal Price { get; set; }
}