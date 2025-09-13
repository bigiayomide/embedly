namespace Embedly.Examples.Infrastructure.Services;

/// <summary>
/// Represents the result of an operation that can succeed or fail.
/// </summary>
/// <typeparam name="T">The type of the result value.</typeparam>
public class Result<T>
{
    public bool IsSuccess { get; init; }
    public T? Value { get; init; }
    public string? Error { get; init; }
    public Exception? Exception { get; init; }

    private Result(bool isSuccess, T? value, string? error, Exception? exception)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
        Exception = exception;
    }

    public static Result<T> Success(T value) => new(true, value, null, null);

    public static Result<T> Failure(string error) => new(false, default, error, null);

    public static Result<T> Failure(Exception exception) => new(false, default, exception.Message, exception);

    public static Result<T> Failure(string error, Exception exception) => new(false, default, error, exception);

    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<string, TResult> onFailure)
    {
        return IsSuccess ? onSuccess(Value!) : onFailure(Error!);
    }

    public async Task<TResult> MatchAsync<TResult>(Func<T, Task<TResult>> onSuccess, Func<string, Task<TResult>> onFailure)
    {
        return IsSuccess ? await onSuccess(Value!) : await onFailure(Error!);
    }
}

/// <summary>
/// Represents the result of an operation that can succeed or fail without a return value.
/// </summary>
public class Result
{
    public bool IsSuccess { get; init; }
    public string? Error { get; init; }
    public Exception? Exception { get; init; }

    private Result(bool isSuccess, string? error, Exception? exception)
    {
        IsSuccess = isSuccess;
        Error = error;
        Exception = exception;
    }

    public static Result Success() => new(true, null, null);

    public static Result Failure(string error) => new(false, error, null);

    public static Result Failure(Exception exception) => new(false, exception.Message, exception);

    public static Result Failure(string error, Exception exception) => new(false, error, exception);

    public TResult Match<TResult>(Func<TResult> onSuccess, Func<string, TResult> onFailure)
    {
        return IsSuccess ? onSuccess() : onFailure(Error!);
    }
}