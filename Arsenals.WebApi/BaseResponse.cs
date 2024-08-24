using System.Text.Json.Serialization;

namespace Arsenals.WebApi;

public class BaseResponse<T>
{

    public static BaseResponse<T> CreateSuccess(T data)
    {
        return new BaseResponse<T>(data)
        {
            Message = null,
            Success = true
        };
    }
    public static BaseResponse<T?> CreateError(Exception exception)
    {
        return new BaseResponse<T?>()
        {
            Message = exception.Message,
            Success = false
        };
    }


    public BaseResponse(T? data)
    {
        Data = data;
    }

    public BaseResponse() { }


    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("data")]
    public T? Data { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }
}
