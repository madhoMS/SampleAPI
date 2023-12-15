using Microsoft.AspNetCore.Http;
using System.Net;

namespace SampleAPI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next) 
        { 
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
           
        }
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Items["Exception"] = exception;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(exception.Message);
        }
    }
}
