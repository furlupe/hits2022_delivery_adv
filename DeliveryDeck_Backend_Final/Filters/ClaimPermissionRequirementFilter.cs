using DeliveryDeck_Backend_Final.Common.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace DeliveryDeck_Backend_Final.Filters
{
    public class ClaimPermissionRequirementAttribute : TypeFilterAttribute
    {
        public ClaimPermissionRequirementAttribute(string value) : base(typeof(ClaimPermissionRequirementFilter))
        {
            Arguments = new[] { new Claim(CustomClaimTypes.Permission, value) };
        }
    }

    public class ClaimPermissionRequirementFilter : IAuthorizationFilter
    {
        readonly string _permission;
        public ClaimPermissionRequirementFilter(Claim claim)
        {
            _permission = claim.Value;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User
                .HasClaim(c => c.Type == CustomClaimTypes.Permission && c.Value == _permission)
                )
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
