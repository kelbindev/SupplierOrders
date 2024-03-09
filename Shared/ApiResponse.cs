namespace Shared;
public class ApiResponse
{
    public bool Success { get; set; } = true;
    public string Message { get; set; } = "Sucess";
    public object? Content { get; set; }

    public static ApiResponse FailResponse(string message)
    {
        return new ApiResponse
        {
            Success = false,
            Message = message
        };
    }

    public static ApiResponse FailResponseWithContent<T>(string message, T content)
    {
        return new ApiResponse
        {
            Success = false,
            Message = message,
            Content = content
        };
    }

    public static ApiResponse SuccessResponse<T>(T content = default, string message = "Success")
    {
        return new ApiResponse
        {
            Success = true,
            Message = message,
            Content = content
        };
    }
}
