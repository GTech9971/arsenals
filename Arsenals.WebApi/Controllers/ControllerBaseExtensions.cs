using System;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Arsenals.WebApi.Controllers;

public static class ControllerBaseExtensions
{
    public static ActionResult<T> Created<T>(this ControllerBase controller, [ActionResultObjectValue] T value)
    {
        return new ContentResult()
        {
            Content = JsonSerializer.Serialize(value),
            ContentType = "application/json",
            StatusCode = (int)HttpStatusCode.Created
        };
    }
}
