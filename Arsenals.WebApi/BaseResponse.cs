using System.Text.Json.Serialization;

namespace Arsenals.WebApi;

public class BaseResponse<T>
{
    public BaseResponse(T? data)
    {
        Data = data;
    }

    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("data")]
    public T? Data { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }
}
