using CbsAp.Application.Exceptions;
using CbsAp.Application.Shared.ResultPatten;
using System.Net;
using System.Net.Mime;
using System.Text.Json;

namespace CbsAp.API.Middlewares.Compliance
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next,
            ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private async Task HandleException(HttpContext context, Exception exception)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            ResponseResult<object> result;

            switch (exception)
            {
                case ArgumentException argumentException:
                    result = ResponseResult<object>.Failure(
                        (int)HttpStatusCode.BadRequest,
                        argumentException.Message);
                    break;

                case AuthenticationException appException:
                    result = ResponseResult<object>.Failure(
                        (int)HttpStatusCode.Unauthorized,
                        appException.Message);
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    result = ResponseResult<object>.Failure(
                        (int)HttpStatusCode.InternalServerError,
                        "An unexpected error occurred. Please check the logs.");
                    break;
            }

            var jsonResponse = JsonSerializer.Serialize(result);
            _logger.LogError(exception, exception.Message, exception.StackTrace);

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}