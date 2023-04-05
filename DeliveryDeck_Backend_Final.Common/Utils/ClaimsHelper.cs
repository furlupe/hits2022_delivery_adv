using System.Security.Claims;

namespace DeliveryDeck_Backend_Final.Common.Utils
{
    public static class ClaimsHelper
    {
        public static Guid GetUserIdFromClaims(IEnumerable<Claim> claims)
        {
            var userIdClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            _ = Guid.TryParse(userIdClaim.Value, out Guid userId);

            return userId;
        }

        public static bool HasPermission(IEnumerable<Claim> claims, string permission)
        {
            return claims.Any(c => c.Type == CustomClaimTypes.Permission && c.Value == permission);
        }
    }
}
