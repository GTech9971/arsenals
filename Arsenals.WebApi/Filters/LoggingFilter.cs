using Microsoft.AspNetCore.Mvc.Filters;

namespace Arsenals.WebApi.Filters;

public class LoggingFilter : IActionFilter
{
    private readonly ILogger<LoggingFilter> _logger;

    public LoggingFilter(ILogger<LoggingFilter> logger)
    {
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var request = context.HttpContext.Request;
        _logger.LogInformation($"[{request.Method}] {request.Scheme}://{request.Host}{request.Path} - Start");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        var request = context.HttpContext.Request;
        var response = context.HttpContext.Response;
        _logger.LogInformation($"[{request.Method}] {request.Scheme}://{request.Host}{request.Path} - {response.StatusCode}");
    }


}
