namespace AdminPanel.Middlewares
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
            try
            {
                await _next(context);
            }
            catch (BadHttpRequestException ex)
            {
                context.Response.StatusCode = ex.StatusCode;
                context.Response.Redirect($"/Home/error?message={ex.Message}");
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.Redirect($"/Home/error?message={ex.Message}");
            }
        }
    }
}
