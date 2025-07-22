using Agora.Domain.Abstractions;

public class Result
{
    public bool IsSuccess { get; }
    public Error? Error { get; }

    public bool IsFailure => !IsSuccess;

    protected Result(bool isSuccess, Error? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new Result(true, null);

    public static Result Failure(Error error) => new Result(false, error);
}

public class Result<T> : Result
{
    public T? Value { get; }

    protected internal Result(T value) : base(true, null)
    {
        Value = value;
    }

    protected internal Result(Error error) : base(false, error)
    {
        Value = default;
    }

    public static Result<T> Success(T value) => new Result<T>(value);

    public static new Result<T> Failure(Error error) => new Result<T>(error);
}
