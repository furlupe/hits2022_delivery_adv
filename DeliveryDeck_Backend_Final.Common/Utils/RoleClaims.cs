using DeliveryDeck_Backend_Final.Common.Enumerations;
using System.Security.Claims;

namespace DeliveryDeck_Backend_Final.Common.Utils
{
    public static class RoleClaims
    {
        public static Dictionary<RoleType, IEnumerable<Claim>> Claims = new()
        {
            { 
                RoleType.Customer, 
                new List<Claim> 
                {
                    new Claim(CustomClaimTypes.Permission, OrderPermissions.Add),
                    new Claim(CustomClaimTypes.Permission, OrderPermissions.ReadOwn),
                    new Claim(CustomClaimTypes.Permission, OrderPermissions.ChangeStatus)
                } 
            },

            {
                RoleType.Manager,
                new List<Claim>
                {
                    new Claim(CustomClaimTypes.Permission, OrderPermissions.Add),
                    new Claim(CustomClaimTypes.Permission, OrderPermissions.ReadRestaurant),
                    new Claim(CustomClaimTypes.Permission, OrderPermissions.ChangeStatus)
                }
            },

            {
                RoleType.Courier,
                new List<Claim>
                {
                    new Claim(CustomClaimTypes.Permission, OrderPermissions.Add),
                    new Claim(CustomClaimTypes.Permission, OrderPermissions.ReadOwn),
                    new Claim(CustomClaimTypes.Permission, OrderPermissions.GetAvailableForDelivery),
                    new Claim(CustomClaimTypes.Permission, OrderPermissions.ChangeStatus)
                }
            },

            {
                RoleType.Cook,
                new List<Claim>
                {
                    new Claim(CustomClaimTypes.Permission, OrderPermissions.Add),
                    new Claim(CustomClaimTypes.Permission, OrderPermissions.ReadOwn),
                    new Claim(CustomClaimTypes.Permission, OrderPermissions.GetAvailableForCooking),
                    new Claim(CustomClaimTypes.Permission, OrderPermissions.ChangeStatus)
                }
            },
        };
    }
}
