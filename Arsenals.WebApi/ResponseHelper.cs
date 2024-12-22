namespace Arsenals.WebApi;

public static class ResponseHelper
{
    public static string? GetErrorMessage(object? response)
    {
        if (response == null) { return null; }

        var errorProperty = response.GetType().GetProperty("Error");
        if (errorProperty == null) { return null; }

        var errorValue = errorProperty.GetValue(response);
        if (errorValue == null) { return null; }

        var messageProperty = errorValue.GetType().GetProperty("Message");
        if (messageProperty == null) { return null; }

        return messageProperty.GetValue(errorValue)?.ToString();
    }
}
