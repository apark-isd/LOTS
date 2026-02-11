using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOTS3.Security
{
    public class SuperAdministratorHandler : AuthorizationHandler<ManageAdminRolesAndClaimsRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            ManageAdminRolesAndClaimsRequirement requirement)
        {
            if(context.User.IsInRole("Super Administrator"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
