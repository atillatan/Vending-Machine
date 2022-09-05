namespace Machine.Application.Common.Dto;

public class ProductDto
{
    public int ProductId { get; set; } = 0;
    
    public string? ProductName { get; set; }

    public decimal? ProductPrice { get; set; } = 0.00m;

    public int RemainingStock { get; set; } = 0;

     public int Capacity { get; set; } = 0;


}
