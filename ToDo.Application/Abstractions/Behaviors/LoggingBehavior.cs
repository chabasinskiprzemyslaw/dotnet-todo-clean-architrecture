using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using ToDo.Application.Abstractions.Messaging;
using ToDo.Domain.Abstractions;

namespace ToDo.Application.Abstractions.Behaviors;

public class LoggingBehavior<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IBaseRequest
    where TResponse : Result
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var name = request.GetType().Name;

        try
        {
            _logger.LogInformation("Executing request {Request}", name);

            var result = await next();

            if (result.IsSuccess)
            {
                _logger.LogInformation("Request {Request} processed successfully", name);
            } 
            else
            {
                using (LogContext.PushProperty("Error", result.Error, true))
                {
                    _logger.LogError("Request {Request} processing failed.", name);
                }
                //_logger.LogError("Request {Request} processing failed. Error: {Error}", name, result.Error);
            }


            return result;
        }
        catch (Exception ex) 
        { 
            _logger.LogError(ex, "Request {Request} processing failed.", name);
            throw;
        }
    }
}

