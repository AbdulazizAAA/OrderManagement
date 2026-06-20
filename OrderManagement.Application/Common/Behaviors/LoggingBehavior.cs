using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrderManagement.Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        _logger.LogInformation("[START] {Request} {@RequestData}", requestName, request);
        var timer = System.Diagnostics.Stopwatch.StartNew();

        try
        {
            var response = await next();
            timer.Stop();
            _logger.LogInformation("[END] {Request} completed in {ElapsedMs}ms", requestName, timer.ElapsedMilliseconds);
            return response;
        }
        catch (Exception ex)
        {
            timer.Stop();
            _logger.LogError(ex, "[ERROR] {Request} failed after {ElapsedMs}ms", requestName, timer.ElapsedMilliseconds);
            throw;
        }
    }
}
