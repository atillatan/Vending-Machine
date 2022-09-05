namespace Machine.Domain.Entities;
public class Coin
{
    [Key]
    [Column(TypeName = "decimal(3,2)")]
    public decimal CoinId { get; set; } = 0;
    public int Count { get; set; } = 0;
}