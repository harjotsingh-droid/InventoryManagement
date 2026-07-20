namespace InventoryManagement.Application.Common;

public class ServiceResult
{
    public bool Success { get; init; }
    public string? ErrorMessage { get; init; }
    public Dictionary<string, string[]>? ValidationErrors { get; init; }

    public static ServiceResult Ok() => new() { Success = true };

    public static ServiceResult Fail(string errorMessage) =>
        new() { Success = false, ErrorMessage = errorMessage };

    public static ServiceResult ValidationFail(Dictionary<string, string[]> errors) =>
        new() { Success = false, ValidationErrors = errors };
}

public class ServiceResult<T> : ServiceResult
{
    public T? Data { get; init; }

    public static ServiceResult<T> Ok(T data) =>
        new() { Success = true, Data = data };

    public new static ServiceResult<T> Fail(string errorMessage) =>
        new() { Success = false, ErrorMessage = errorMessage };

    public new static ServiceResult<T> ValidationFail(Dictionary<string, string[]> errors) =>
        new() { Success = false, ValidationErrors = errors };
}
