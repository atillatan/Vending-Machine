using Machine.Application.Common.Interfaces;
using Machine.Application.Common.Exceptions;
using Serilog;
namespace Machine.Application.Products.Commands.ProductDelete;

public class ProductDeleteHandler : IRequestHandler<ProductDeleteCommand, bool>
{
    private readonly ILanguageService _lang;

    private readonly ILogger _logger = Log.ForContext<ProductDeleteHandler>();

    private readonly IApplicationDbContext _dBContext;

    public ProductDeleteHandler(ILanguageService languageService, IApplicationDbContext dbContext)
    {
        _dBContext = dbContext;
        _lang = languageService;
    }

    public async Task<bool> Handle(ProductDeleteCommand command, CancellationToken cancellationToken)
    {
        var entity = await _dBContext.Product.FindAsync(new string[] { command.ProductId, command.SlotId }, cancellationToken);
        if (entity == null) throw new NotFoundException(_lang.Translate("ERR_DATA_NOT_FOUND"));

        _dBContext.Product.Remove(entity);
        int result = await _dBContext.SaveChangesAsync(cancellationToken);
        _logger.Debug($"operation completed Command:{command}");
        return result > 0;
    }
}
