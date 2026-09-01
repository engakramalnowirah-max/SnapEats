using MediatR;
using Microsoft.Extensions.Logging;

namespace SnapEats.Application.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var requestId = Guid.NewGuid().ToString();

        _logger.LogInformation("Processing request {RequestId}: {RequestName} {@Request}",
            requestId, requestName, request);

        try
        {
            var response = await next();
            _logger.LogInformation("Completed request {RequestId}: {RequestName}", requestId, requestName);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed request {RequestId}: {RequestName}", requestId, requestName);
            throw;
        }
    }
}

