using Microsoft.AspNetCore.Http;

namespace DeliveryDeck_Backend_Final.Common
{
    public class ExceptionRedirectMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionRedirectMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var message = "";
            try
            {
                await _next(context);
            }
            catch (BadHttpRequestException ex)
            {
                context.Response.StatusCode = ex.StatusCode;
                message = ex.Message;
                context.Response.Redirect($"/Home/error?message={message}");
            }
            catch
            {
                throw;
            }
        }
    }
}
