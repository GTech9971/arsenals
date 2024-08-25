using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Arsenals.WebApi.Filters;

public class ExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ExceptionFilter> _logger;

    public ExceptionFilter(ILogger<ExceptionFilter> logger)
    {
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        _logger = logger;
    }
    public void OnException(ExceptionContext context)
    {
        Exception exception = context.Exception;

        _logger.LogError(exception, exception.Message);

        BaseResponse<object?> response = BaseResponse<object?>.CreateError(exception);


        string contentJson = JsonSerializer.Serialize(response);
        context.Result = new ContentResult
        {
            Content = contentJson,
            ContentType = "application/json",
            StatusCode = (int)HttpStatusCode.InternalServerError
        };
    }
}
