using Microsoft.AspNetCore.Authorization;
using RestaurantApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Application.Authorization
{
    public class ResourceActionsRequirementHandler : AuthorizationHandler<ResourceActionsRequirement, Restaurant>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceActionsRequirement requirement, Restaurant resource)
        {
            if (requirement.ResourceAction == ResourceAction.Read ||
                requirement.ResourceAction == ResourceAction.Create)
            {
                context.Succeed(requirement);
            }

            var userId = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            if (resource.CreatedById == userId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
