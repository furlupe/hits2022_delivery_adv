using DeliveryDeck_Backend_Final.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.TagHelpers;

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
            catch (RepositoryEntityAlreadyExistsException ex)
            {
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                message = ex.Message;
                context.Response.Redirect($"/Home/error?message={message}");
            }
            catch (RepositoryEntityNotFoundException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
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
