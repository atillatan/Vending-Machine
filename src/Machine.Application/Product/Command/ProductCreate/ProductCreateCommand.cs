using Machine.Application.Products;

namespace Machine.Application.Products.Commands.ProductCreate;

public class ProductCreateCommand : IRequest<ProductDto>
{

    public string SlotId { get; set; } = default!;
    public int ProductId { get; set; }
    public string ProductName { get; set; } = default!;
    public int RemainingStock { get; set; }
    public int Capacity { get; set; }
}

public class ProductCreateCommandValidator : AbstractValidator<ProductCreateCommand>
{
    public ProductCreateCommandValidator()
    {
        RuleFor(m => m.ProductId).NotNull().NotEmpty();
        RuleFor(m => m.ProductName).NotNull().NotEmpty();
        RuleFor(m => m.Capacity).NotNull().NotEmpty();
    }
}