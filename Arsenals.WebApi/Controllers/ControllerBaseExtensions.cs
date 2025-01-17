using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;

namespace Arsenals.WebApi.Controllers;

public static class ControllerBaseExtensions
{
    public static ActionResult<T> Created<T>(this ControllerBase _, [ActionResultObjectValue] T value)
    {
        return new ContentResult()
        {
            Content = JsonConvert.SerializeObject(value),
            ContentType = "application/json",
            StatusCode = (int)HttpStatusCode.Created
        };
    }
}
