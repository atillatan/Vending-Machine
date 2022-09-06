using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
namespace Machine.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        //pre behavior process => GenericRequestPreProcessor()
        return await next();
        //post behavior process => GenericRequestPostProcessor()
    }
}

public class GenericRequestPreProcessor<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger<GenericRequestPreProcessor<TRequest>> _logger;


    public GenericRequestPreProcessor(ILogger<GenericRequestPreProcessor<TRequest>> logger)
    {
        _logger = logger;    
    }

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;     
        _logger.LogInformation("Request Started");
        return Task.CompletedTask;
    }
}

public class GenericRequestPostProcessor<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<GenericRequestPostProcessor<TRequest, TResponse>> _logger;


    public GenericRequestPostProcessor(ILogger<GenericRequestPostProcessor<TRequest, TResponse>> logger)
    {
        _logger = logger;      
    }
    public Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
    {
         _logger.LogInformation("Request Finished");
        return Task.CompletedTask;
    }
}