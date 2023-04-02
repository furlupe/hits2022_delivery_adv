using DeliveryDeck_Backend_Final.Common.CustomPermission;
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
                    new Claim(CustomClaimTypes.Permission, OrderPermissions.ReadOwnOrderHistory),
                    new Claim(CustomClaimTypes.Permission, OrderPermissions.Cancel),
                    new Claim(CustomClaimTypes.Permission, CartPermissions.Read),
                    new Claim(CustomClaimTypes.Permission, CartPermissions.Adjust),
                    new Claim(CustomClaimTypes.Permission, RatingPermissions.Read),
                    new Claim(CustomClaimTypes.Permission, RatingPermissions.Add),
                    new Claim(CustomClaimTypes.Permission, RatingPermissions.Update)
                } 
            },

            {
                RoleType.Manager,
                new List<Claim>
                {
                    new Claim(CustomClaimTypes.Permission, OrderPermissions.ReadRestaurant)                
                }
            },

            {
                RoleType.Courier,
                new List<Claim>
                {
                    new Claim(CustomClaimTypes.Permission, OrderPermissions.Add),
                    new Claim(CustomClaimTypes.Permission, OrderPermissions.ReadOwnDeliveryHistory),
                    new Claim(CustomClaimTypes.Permission, OrderPermissions.GetAvailableForDelivery),
                    new Claim(CustomClaimTypes.Permission, OrderPermissions.ChangeStatusToDelivered)
                }
            },

            {
                RoleType.Cook,
                new List<Claim>
                {
                    new Claim(CustomClaimTypes.Permission, OrderPermissions.Add),
                    new Claim(CustomClaimTypes.Permission, OrderPermissions.ReadOwnCookingHistory),
                    new Claim(CustomClaimTypes.Permission, OrderPermissions.GetAvailableForCooking),
                    new Claim(CustomClaimTypes.Permission, OrderPermissions.ChangeStatusUntilDelivery)
                }
            },
        };
    }
}
