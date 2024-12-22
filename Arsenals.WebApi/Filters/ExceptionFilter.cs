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
        var request = context.HttpContext.Request;

        string? errorMessage = null;
        if (context.Result is ObjectResult result)
        {
            errorMessage = ResponseHelper.GetErrorMessage(result.Value);
        }

        _logger.LogError(exception, exception.Message);
        _logger.LogError($"[{request.Method}][500] {request.Scheme}://{request.Host}{request.Path} {errorMessage}");

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
