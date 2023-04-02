using System.Security.Claims;

namespace DeliveryDeck_Backend_Final.Common.Utils
{
    public static class ClaimsHelper
    {
        public static Guid GetUserId(IEnumerable<Claim> claims)
        {
            var userIdClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            _ = Guid.TryParse(userIdClaim.Value, out Guid userId);

            return userId;
        }
    }
}
