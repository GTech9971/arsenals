using System.Net;
using Microsoft.AspNetCore.Mvc;
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
        int? statusCode = null;
        string? errorMessage = null;

        if (context.Result is ObjectResult result)
        {
            statusCode = result.StatusCode;
            errorMessage = ResponseHelper.GetErrorMessage(result.Value);
        }
        else if (context.Result is NoContentResult)
        {
            statusCode = (int)HttpStatusCode.NoContent;
        }

        if (statusCode >= 100 && statusCode < 400)
        {
            _logger.LogInformation($"[{request.Method}][{statusCode}] {request.Scheme}://{request.Host}{request.Path} - end");
        }
        else if (statusCode >= 400 && statusCode < 500)
        {
            _logger.LogWarning($"[{request.Method}][{statusCode}] {request.Scheme}://{request.Host}{request.Path} - end {errorMessage}");
        }
    }


}
