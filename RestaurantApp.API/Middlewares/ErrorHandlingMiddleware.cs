

using RestaurantApp.Application.Exceptions;

namespace RestaurantApp.API.Middlewares
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (ForbidException e)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync(e.Message);

            }
            catch (NotFoundException e)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(e.Message);
               
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);

                if (_webHostEnvironment.IsDevelopment())
                {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync($"{e.Message}, \n{e.StackTrace}");
                }
                else
                {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync("Something went wrong");
                }
            }
        }
    }
}
