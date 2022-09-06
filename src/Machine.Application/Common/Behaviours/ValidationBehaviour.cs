using Machine.Application.Common.Interfaces;
using ValidationException = Machine.Application.Common.Exceptions.ValidationException;
namespace Machine.Application.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILanguageService _languageService;
    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators, ILanguageService languageService)
    {
        _validators = validators;
        _languageService = languageService;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {

        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

            if (failures.Count != 0)
                throw new ValidationException(failures, _languageService.Translate("ERR_VALIDATION_GENERAL"));
        }
        return await next();
    }
}
