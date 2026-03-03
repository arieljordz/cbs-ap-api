using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace CbsAp.Application.Shared
{
    /// <summary>
    /// Result Object Pattern : represent the result of an operation.
    /// Note : We can add additiona scenario if We needed
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [ExcludeFromCodeCoverage]
    [Obsolete("This is deprecated introduce new pattern")]
    public class Result<T>
    {
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string? Messages { get; set; }

        public Result(bool isSuccess, T data, string messages)
        {
            IsSuccess = isSuccess;
            Data = data;
            Messages = messages;
        }

        public Result(int statusCode, bool isSuccess, T data, string messages)
        {
            StatusCode = statusCode;
            IsSuccess = isSuccess;
            Data = data;
            Messages = messages;
        }
        public Result()
        {
            
        }
        public static Result<T> NotFound(string errorMessage)
        {
            return new Result<T>((int)HttpStatusCode.NotFound, false, default!, errorMessage);
        }

        public static Result<T> AlreadyExists(string errorMessage)
        {
            return new Result<T>((int)HttpStatusCode.Conflict, false, default!, errorMessage);
        }
        public static Result<T> Success()
        {
            return new Result<T>(true, default!, null!);
        }
        public static Result<T> Success(string message)
        {
            return new Result<T>(true, default!, message);
        }

        public static Result<T> Success(T data)
        {
            return new Result<T>(true, data, null!);
        }

        public static Result<T> Success(T data, string message)
        {
            return new Result<T>(true, data, message);
        }

        public static Result<T> Success(int statusCode, T data)
        {
            return new Result<T>(statusCode, true, data, null!);
        }

        public static Result<T> Success(int statusCode, T data, string message)
        {
            return new Result<T>(statusCode, true, data, message);
        }

        public static Result<T> Failure(string errorMessage)
        {
            return new Result<T>(false, default!, errorMessage);
        }

        public static Result<T> Failure(Exception exception)
        {
            return new Result<T>(false, default!, exception.Message);
        }

        public static Result<T> Failure(int statusCode, string errorMessage)
        {
            return new Result<T>(statusCode, false, default!, errorMessage);
        }

        public static Result<T> Failure(int statusCode, Exception exception)
        {
            return new Result<T>(statusCode, false, default!, exception.Message);
        }

        public static Task<Result<T>> SuccessAsync()
        {
            return Task.FromResult(Success());
        }
        public static Task<Result<T>> SuccessAsync(string message)
        {
            return Task.FromResult(Success(message));
        }

        public static Task<Result<T>> SuccessAsync(T data)
        {
            return Task.FromResult(Success(data));
        }

        public static Task<Result<T>> SuccessAsync(T data, string message)
        {
            return Task.FromResult(Success(data, message));
        }

        public static Task<Result<T>> SuccessAsync(int statusCode, T data)
        {
            return Task.FromResult(Success(statusCode, data));
        }

        public static Task<Result<T>> SuccessAsync(int statusCode, T data, string message)
        {
            return Task.FromResult(Success(statusCode, data, message));
        }

        public static Task<Result<T>> FailureAsync(string errorMessage)
        {
            return Task.FromResult(Failure(errorMessage));
        }       

        public static Task<Result<T>> FailureAsync(Exception exception)
        {
            return Task.FromResult(Failure(exception));
        }

        public static Task<Result<T>> FailureAsync(int statusCode, string errorMessage)
        {
            return Task.FromResult(Failure(statusCode, errorMessage));
        }

        public static Task<Result<T>> FailureAsync(int statusCode, Exception exception)
        {
            return Task.FromResult(Failure(statusCode, exception));
        }
    }
}