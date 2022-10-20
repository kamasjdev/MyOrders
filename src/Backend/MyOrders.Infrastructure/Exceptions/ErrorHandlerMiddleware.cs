using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MyOrders.Application.Exceptions;
using MyOrders.Core.Exceptions;
using System.Net;

namespace MyOrders.Infrastructure.Exceptions
{
    internal sealed class ErrorHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(ILogger<ErrorHandlerMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                await HandleErrorAsync(context, exception);
            }
        }

        private static async Task HandleErrorAsync(HttpContext context, Exception exception)
        {
            var (error, code) = Map(exception);
            context.Response.StatusCode = (int)code;
            await context.Response.WriteAsJsonAsync(error);
        }

        private static (Error error, HttpStatusCode code) Map(Exception exception)
           => exception switch
           {
               DomainException ex => (new Error(ex.Message), HttpStatusCode.BadRequest),
               BusinessException ex => (new Error(ex.Message), HttpStatusCode.BadRequest),
               _ => (new Error("There was an error."), HttpStatusCode.InternalServerError)
           };

        private record Error(string Message);
    }
}
