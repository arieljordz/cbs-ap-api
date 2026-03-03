using CbsAp.Application.Shared;
using Ganss.Xss;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;
using System.Text.Json;

namespace CbsAp.API.Middlewares.Compliance
{
    [ExcludeFromCodeCoverage]
    public class ApplicationAntiXssMiddleware
    {
        private readonly RequestDelegate _next;
        private ErrorResponse? _errorResponse;
        private readonly int _statusCode = (int)HttpStatusCode.BadRequest;
        private readonly ILogger<ApplicationAntiXssMiddleware> _logger;

        public ApplicationAntiXssMiddleware(RequestDelegate next, ILogger<ApplicationAntiXssMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        private static async Task<string> ReadRequestBody(HttpContext context)
        {
            var buffer = new MemoryStream();
            await context.Request.Body.CopyToAsync(buffer);
            context.Request.Body = buffer;
            buffer.Position = 0;

            var encoding = Encoding.UTF8;

            var requestContent = await new StreamReader(buffer, encoding).ReadToEndAsync();
            context.Request.Body.Position = 0;

            return requestContent;
        }

        private async Task RespondWithAnError(HttpContext context)
        {
            context.Response.Clear();
            context.Response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            context.Response.StatusCode = _statusCode;

            if (_errorResponse == null)
            {
                _errorResponse = new ErrorResponse(_statusCode, "XSS Detected.");
            }

            _logger.LogError($"Error Code :{_errorResponse.ErrorCode} Message: {_errorResponse.Message} ");
            await context.Response.WriteAsync(JsonSerializer.Serialize(_errorResponse));
        }

        public async Task Invoke(HttpContext context)
        {
            var originalBody = context.Request.Body;
            try
            {
                if (context.Request.ContentType?.StartsWith("multipart/form-data") == true)
                {
                    await _next(context);
                    return;
                }

                context.Request.EnableBuffering();

                // Read the body as string
                var content = await ReadRequestBody(context);

                var sanitizer = new HtmlSanitizer();
                var sanitized = sanitizer.Sanitize(content);

                if (content != sanitized.Replace("&amp;", "&"))
                {
                    await RespondWithAnError(context);
                    return; // stop the pipeline here
                }

                await _next(context);
            }
            finally
            {
                context.Request.Body = originalBody;
            }
        }
    }
}