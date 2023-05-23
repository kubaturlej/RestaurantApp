using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Application.Authorization
{
    public enum ResourceAction
    {
        Create,
        Read,
        Update,
        Delete
    }

    public class ResourceActionsRequirement : IAuthorizationRequirement
    {
        public ResourceActionsRequirement(ResourceAction resourceAction)
        {
            ResourceAction = resourceAction;
        }

        public ResourceAction ResourceAction { get; }
    }
}
