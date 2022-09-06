using Serilog;
namespace Machine.Application.Common.Behaviours;

public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger _logger = Log.ForContext<TRequest>();

    public UnhandledExceptionBehaviour()
    { }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;

            _logger.Error(ex, $"Vending Machine Request: Unhandled Exception for Request {request.ToString()} Exception.Message:{ex.Message}, Exception:{ex.ToString()}", requestName, request!);

            throw;
        }
    }
}
