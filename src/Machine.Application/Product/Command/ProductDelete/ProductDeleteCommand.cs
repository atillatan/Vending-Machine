namespace Machine.Application.Products.Commands.ProductDelete;

public class ProductDeleteCommand : IRequest<bool>
{
    public string ProductId { get; set; } = default!;
    public string SlotId { get; set; } = default!;
}

public class ProductDeleteCommandValidator : AbstractValidator<ProductDeleteCommand>
{
    public ProductDeleteCommandValidator()
    {
        RuleFor(m => m.ProductId).NotEmpty();
        RuleFor(m => m.SlotId).NotEmpty();
    }
}
