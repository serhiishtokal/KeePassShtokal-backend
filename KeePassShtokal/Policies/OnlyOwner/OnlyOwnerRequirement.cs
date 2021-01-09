using Microsoft.AspNetCore.Authorization;

namespace KeePassShtokal.Policies.OnlyOwner
{
    public class OnlyOwnerRequirement : IAuthorizationRequirement
    {
    }
}
