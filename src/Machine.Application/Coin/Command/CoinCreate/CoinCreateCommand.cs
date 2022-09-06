using Machine.Application.Coins;

namespace Machine.Application.Coins.Commands.CoinCreate;

public class CoinCreateCommand : IRequest<CoinDto>
{
    
    public decimal CoinId { get; set; }
    public int Count { get; set; }
}

public class CoinCreateCommandValidator : AbstractValidator<CoinCreateCommand>
{
    public CoinCreateCommandValidator()
    {
        RuleFor(m => m.CoinId).NotNull().NotEmpty();
        RuleFor(m => m.Count).NotNull().NotEmpty();        
    }
}