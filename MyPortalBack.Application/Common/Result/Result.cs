using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortalBack.Application.Common.Result;

public class Result<T>
{
    public bool Success { get; }

    public T? Value { get; }

    public IReadOnlyCollection<string> Errors { get; }

    protected Result(
        bool success,
        T? value,
        IReadOnlyCollection<string> errors)
    {
        Success = success;
        Value = value;
        Errors = errors;
    }

    public static Result<T> Ok(T value)
    {
        return new Result<T>(
            true,
            value,
            Array.Empty<string>());
    }

    public static Result<T> Failure(params string[] errors)
    {
        return new Result<T>(
            false,
            default,
            errors);
    }

    public static Result<T> Failure(IEnumerable<string> errors)
    {
        return new Result<T>(
            false,
            default,
            errors.ToArray());
    }
}
