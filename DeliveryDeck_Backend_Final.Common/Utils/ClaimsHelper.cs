using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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
    }
}
