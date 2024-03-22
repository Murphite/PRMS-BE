namespace PRMS.Core.Dtos;

public class Result<TValue> : Result where TValue : class
{
    private readonly TValue? _data;

    public Result(TValue? data, bool isSuccess, IEnumerable<Error> errors) : base(isSuccess, errors)
    {
        _data = data;
    }

    public TValue Data => _data!;

    public static implicit operator Result<TValue>(TValue value)
    {
        return Success(value);
    }

    // return Result.Success(data);
    // return data;
    public static implicit operator TValue(Result<TValue> result)
    {
        if (result.IsSuccess) return result.Data;

        throw new InvalidOperationException("Cannot convert a failed result to a value.");
    }

    public static implicit operator Result<TValue>(Error[] errors)
    {
        return Failure<TValue>(errors);
    }
}

public class Result
{
    protected Result(bool isSuccess, IEnumerable<Error> errors)
    {
        if (isSuccess && errors.Any())
            throw new InvalidOperationException("cannot be successful with error");
        if (!isSuccess && !errors.Any())
            throw new InvalidOperationException("cannot be unsuccessful without error");

        IsSuccess = isSuccess;
        Errors = errors;
        IsFailure = !isSuccess;
    }

    public bool IsSuccess { get; }
    public bool IsFailure { get; }

    public IEnumerable<Error> Errors { get; }

    public static Result Success()
    {
        return new Result(true, Error.None);
    }

    // return Result.Success();

    public static Result<TValue> Success<TValue>(TValue value) where TValue : class
    {
        return new Result<TValue>(value, true, Error.None);
    }

    // return Result.Success(data)

    public static Result Failure(IEnumerable<Error> errors)
    {
        return new Result(false, errors);
    }

    // return new Result(false, errors)
    // return Result.Failure(errors)

    public static Result<TValue> Failure<TValue>(IEnumerable<Error> errors) where TValue : class
    {
        return new Result<TValue>(null, false, errors);
    }

    public static implicit operator Result(Error[] errors)
    {
        return Failure(errors);
    }
}