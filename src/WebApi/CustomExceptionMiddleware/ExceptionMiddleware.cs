using Infrastructure.Exceptions;
using WebApi.Dtos;

namespace WebApi.CustomExceptionMiddleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            string errorMessage;
            context.Response.ContentType = "application/json";
            switch (exception)
            {
                case MessageNotFoundException:
                    context.Response.StatusCode = 404;
                    errorMessage = exception.Message;
                    break;
                default:
                    context.Response.StatusCode = 500;
                    errorMessage = $"Internal Server Error: something went wrong on the server";
                    break;
            }
            await context.Response.WriteAsync(new ResponseDetails(context.Response.StatusCode, errorMessage).ToString());
        }
    }
}
