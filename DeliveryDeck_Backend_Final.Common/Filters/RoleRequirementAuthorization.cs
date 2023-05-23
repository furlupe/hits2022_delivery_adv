using DeliveryDeck_Backend_Final.Common.Enumerations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace DeliveryDeck_Backend_Final.Common.Filters
{
    public class RoleRequirementAuthorization
    {
        public class RoleRequirementAuthorizationAttribute : TypeFilterAttribute
        {
            public RoleRequirementAuthorizationAttribute(RoleType role) : base(typeof(RoleRequirementAuthorizationFilter))
            {
                Arguments = new[] { new Claim(ClaimTypes.Role, role.ToString()) };
            }
        }

        public class RoleRequirementAuthorizationFilter : IAuthorizationFilter
        {
            readonly string _role;
            public RoleRequirementAuthorizationFilter(Claim claim)
            {
                _role = claim.Value;
            }
            public void OnAuthorization(AuthorizationFilterContext context)
            {
                if (!context.HttpContext.User
                    .HasClaim(c => c.Type == ClaimTypes.Role && c.Value == _role)
                    )
                {
                    context.Result = new ForbidResult();
                }
            }
        }
    }
}
