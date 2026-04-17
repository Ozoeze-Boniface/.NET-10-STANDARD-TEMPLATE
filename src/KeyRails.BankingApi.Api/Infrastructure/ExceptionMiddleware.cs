using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using KeyRails.BankingApi.Application.Common.Exceptions;
using KeyRails.BankingApi.Application.Common.Models.View;

namespace KeyRails.BankingApi.Api.Infrastructure
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
                var requestTime = DateTime.Now;
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, requestTime);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception, DateTime requestTime)
        {
            var code = HttpStatusCode.InternalServerError;
            var result = string.Empty;

            switch (exception)
            {
                case ValidationException:
                    code = HttpStatusCode.OK;
                    break;
                case BadRequestException :
                case EntityNotFoundException:
                case ConflictException:
                    code = HttpStatusCode.BadRequest;
                    result = JsonConvert.SerializeObject(
                        Result<dynamic>.Failure(requestTime, exception.Message, exception.HResult)
                    );
                    break;
                case UnauthorizedException:
                    code = HttpStatusCode.Unauthorized;
                    result = JsonConvert.SerializeObject(
                        Result<dynamic>.Failure(requestTime, exception.Message)
                    );
                    break;

                case ForbiddenAccessException:
                    code = HttpStatusCode.Forbidden;
                    result = JsonConvert.SerializeObject(
                        Result<dynamic>.Failure(requestTime, exception.Message)
                    );
                    break;
            }

            _logger.LogError(exception, exception.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            if (  exception is ValidationException ex)
            {
                result = JsonConvert.SerializeObject(Result<dynamic>.Failure(requestTime, exception.Message, ex.Errors));
            }
            if (string.IsNullOrEmpty(result))
            {
                result = JsonConvert.SerializeObject(Result<dynamic>.Failure(requestTime, exception.Message));
            }

            return context.Response.WriteAsync(result);
        }
    }

    public static class CustomExceptionHandlerMiddlewareExtension
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
