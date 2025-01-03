﻿using System.Diagnostics.CodeAnalysis;

namespace ToDo.Domain.Abstractions;

public class Result
{
    protected internal Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException();
        }

        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    public static Result Success() => new Result(true, Error.None);

    public static Result Failure(Error error) => new Result(false, error);

    public static Result<T> Success<T>(T value) => new Result<T>(value, true, Error.None);
    public static Result<T> Failure<T>(Error error) => new Result<T>(default!, false, error);

}

public class Result<T> : Result
{
    private readonly T? _value;

    protected internal Result(T value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    [NotNull]
    public T Value => IsSuccess ? _value! : throw new InvalidOperationException("The value of failure result can be accessed.");

    public static implicit operator T(Result<T> result) => result.Value;

}
