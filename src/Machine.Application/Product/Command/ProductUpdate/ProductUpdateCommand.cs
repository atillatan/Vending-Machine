using Machine.Application.Products;
using Machine.Application.Products;

namespace Machine.Application.Products.Commands.ProductUpdate;

public class ProductUpdateCommand : IRequest<ProductDto>
{
public string SlotId {get;set;} = default!;
public int ProductId {get;set;} = default!;
public string ProductName {get;set;} = default!;
public int ProductPrice {get;set;} = default!;
public int RemainingStock {get;set;} = default!;
public int Capacity {get;set;} = default!;
}

public class ProductUpdateCommandValidator : AbstractValidator<ProductUpdateCommand>
{
    public ProductUpdateCommandValidator()
    {
        RuleFor(m => m.ProductId).NotNull().NotEmpty();
        RuleFor(m => m.SlotId).NotNull().NotEmpty();
        RuleFor(m => m.ProductName).NotNull().NotEmpty();
    }
}
