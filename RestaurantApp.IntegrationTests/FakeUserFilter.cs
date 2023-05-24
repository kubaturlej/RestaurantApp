using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace RestaurantApp.IntegrationTests
{
    public class FakeUserFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var claimsPrinciple = new ClaimsPrincipal();
            claimsPrinciple.AddIdentity(new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "123"),
                    new Claim(ClaimTypes.Role, "Admin")
                }));
            context.HttpContext.User = claimsPrinciple;

            await next();
        }
    }
}
