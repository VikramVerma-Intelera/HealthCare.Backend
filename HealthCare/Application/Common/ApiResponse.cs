namespace HealthCare.Application.Common;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public int? StatusCode { get; set; }

    public ApiResponse(bool success, string message, T? data = default, int? statusCode = null)
    {
        Success = success;
        Message = message;
        Data = data;
        StatusCode = statusCode ?? (success ? 200 : 400);
    }

    public static ApiResponse<T> SuccessResponse(T data, string message = "Operation successful", int statusCode = 200)
        => new(true, message, data, statusCode);

    public static ApiResponse<T> ErrorResponse(string message, int statusCode = 400)
        => new(false, message, default, statusCode);

    public static ApiResponse<T> NotFoundResponse(string message = "Resource not found")
        => new(false, message, default, 404);

    public static ApiResponse<T> CreatedResponse(T data, string message = "Resource created successfully")
        => new(true, message, data, 201);
}
