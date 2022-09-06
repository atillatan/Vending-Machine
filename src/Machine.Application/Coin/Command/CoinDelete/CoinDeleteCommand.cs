namespace Machine.Application.Coins.Commands.CoinDelete;

public class CoinDeleteCommand : IRequest<bool>
{
    public decimal CoinId { get; set; } = default!;
    
}

public class CoinDeleteCommandValidator : AbstractValidator<CoinDeleteCommand>
{
    public CoinDeleteCommandValidator()
    {
        RuleFor(m => m.CoinId).NotEmpty();
         
    }
}
