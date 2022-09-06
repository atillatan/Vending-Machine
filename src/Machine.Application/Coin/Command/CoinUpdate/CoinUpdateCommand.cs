namespace Machine.Application.Coins.Commands.CoinUpdate;

public class CoinUpdateCommand : IRequest<CoinDto>
{
public int Count {get;set;} = default!;
public decimal CoinId {get;set;} = default!;

}

public class CoinUpdateCommandValidator : AbstractValidator<CoinUpdateCommand>
{
    public CoinUpdateCommandValidator()
    {
        RuleFor(m => m.CoinId).NotNull().NotEmpty();
        RuleFor(m => m.Count).NotNull().NotEmpty();      
    }
}
