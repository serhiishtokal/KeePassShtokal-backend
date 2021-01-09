using System;
using System.Security.Claims;
using System.Threading.Tasks;
using KeePassShtokal.Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;

namespace KeePassShtokal.Policies.OnlyOwner
{
    public class OnlyOwnerHandler : AuthorizationHandler<OnlyOwnerRequirement, Entry>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OnlyOwnerRequirement requirement, Entry entry)
        {
            //var userIdString = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //Guid.TryParse(userIdString, out Guid userId);

            //var userOwner

            //if (entry. == userId)
            //{
            //    context.Succeed(requirement);
            //}
            return Task.CompletedTask;
        }
    }
}
