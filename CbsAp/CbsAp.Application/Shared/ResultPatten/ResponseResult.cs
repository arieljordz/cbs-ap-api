using CbsAp.Application.Configurations.constants;
using CbsAp.Domain.Enums;
using System.Net;

namespace CbsAp.Application.Shared.ResultPatten
{
    public class ResponseResult<T> : BaseResult
    {
        public int StatusCode { get; set; }
        public T ResponseData { get; set; }

        public ResponseResult(
            int statuscode,
             bool isSucess,
            T responseData,
            List<string> messages)
        {
            StatusCode = statuscode;
            IsSuccess = isSucess;
            ResponseData = responseData;
            Messages = messages ?? new List<string>();
        }

        public static ResponseResult<T> Success(T responseData,
            int statusCode = (int)HttpStatusCode.OK,
            params string[] messages)
        {
            return new ResponseResult<T>(
                 statusCode
               , true

                , responseData
                , messages.ToList());
        }

        public static ResponseResult<T> Success(int statusCode, T? data, string message)
        {
            return new ResponseResult<T>(statusCode,
                true,
                data!,
                new List<string> { message });
        }

        public static ResponseResult<T> Success(int statusCode, string message)
        {
            return new ResponseResult<T>(
                statusCode,
                true,
                default!,
                new List<string> { message });
        }

        public static ResponseResult<T> Success(T? data, string message)
        {
            return new ResponseResult<T>((int)HttpStatusCode.OK,
                true,
                data!,
                new List<string> { message });
        }

        public static ResponseResult<T> Failure(T responseData,
           int statusCode,
           params string[] messages)
        {
            return new ResponseResult<T>(
                  statusCode
               , false
                , responseData
                , messages.ToList());
        }

        // Overload for common HTTP status codes
        public static ResponseResult<T> Failure(int statusCode, string message)
        {
            return new ResponseResult<T>(statusCode, false, default!,
                new List<string> { message });
        }

        // Common Success Response

        private static ResponseResult<T> RetrieveResponse(T data, string[] parameter, MessageOperationType operationType)
        {
            return Success(data, MessageConstants.Message(operationType, parameter));
        }

        public static ResponseResult<T> SuccessResponse(int statusCode, string parameter,
            MessageOperationType operationType)
        {
            return Success(statusCode, MessageConstants.Message(parameter, operationType));
        }

        public static ResponseResult<T> NotFound(params string[] messages)
        {
            return Failure(default!, (int)HttpStatusCode.NotFound, messages);
        }

        public static ResponseResult<T> BadRequest(params string[] messages)
        {
            return Failure(default!, (int)HttpStatusCode.BadRequest, messages);
        }

        public static ResponseResult<T> BadRequest(T responseData, params string[] messages)
        {
            return Failure(responseData, (int)HttpStatusCode.BadRequest, messages);
        }

        public static ResponseResult<T> Unauthorized(params string[] messages)
        {
            return Failure(default!, (int)HttpStatusCode.Unauthorized, messages);
        }

        public static ResponseResult<T> InternalServerError(params string[] messages)
        {
            return Failure(default!, (int)HttpStatusCode.InternalServerError, messages);
        }

        public static ResponseResult<T> Confict(params string[] messages)
        {
            return Failure(default!, (int)HttpStatusCode.Conflict, messages);
        }

        public static ResponseResult<T> Created(string message)
        {
            return SuccessResponse((int)HttpStatusCode.Created, message, MessageOperationType.Create);
        }

        public static ResponseResult<T> Created(T responseData, params string[] messages)
        {
            return Success(responseData, (int)HttpStatusCode.Created, messages);
        }

        public static ResponseResult<T> OK(T responseData, params string[] messages)
        {
            return Success(responseData, (int)HttpStatusCode.OK, messages);
        }

        public static ResponseResult<T> OK(string message)
        {
            return Success((int)HttpStatusCode.OK, message);
        }

        public static ResponseResult<T> SuccessRetrieveRecords(T responseData, params string[] messages)
        {
            return RetrieveResponse(responseData, messages, MessageOperationType.Retrieve);
        }
    }
}