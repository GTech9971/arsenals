using System;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Arsenals.WebApi.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        Exception exception = context.Exception;

        BaseResponse<object?> response = new BaseResponse<object?>(null);
        response.Success = false;
        response.Message = exception.Message;


        string contentJson = JsonSerializer.Serialize(response);
        context.Result = new ContentResult
        {
            Content = contentJson,
            ContentType = "application/json",
            StatusCode = (int)HttpStatusCode.InternalServerError
        };
    }
}
