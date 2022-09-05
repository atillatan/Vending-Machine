namespace Machine.Domain.Entities;

public class Product
{
    [Key] public string SlotId { get; set; } = default!;

    [Key]
    public int ProductId { get; set; } = default!;
    public string ProductName { get; set; } = default!;
    public int RemainingStock { get; set; }
    public int Capacity { get; set; }
}