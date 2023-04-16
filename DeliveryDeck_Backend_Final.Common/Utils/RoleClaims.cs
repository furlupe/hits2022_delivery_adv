using DeliveryDeck_Backend_Final.Common.CustomPermissions;
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
                    new Claim(CustomClaimTypes.Permission, OrderPermissions.ReadRestaurantHistory),
                    new Claim(CustomClaimTypes.Permission, MenuPermissions.Read),
                    new Claim(CustomClaimTypes.Permission, MenuPermissions.Create),
                    new Claim(CustomClaimTypes.Permission, MenuPermissions.Adjust),
                    new Claim(CustomClaimTypes.Permission, DishPermissions.Read),
                    new Claim(CustomClaimTypes.Permission, DishPermissions.CUD)
                }
            },

            {
                RoleType.Courier,
                new List<Claim>
                {
                    new Claim(CustomClaimTypes.Permission, OrderPermissions.Add),
                    new Claim(CustomClaimTypes.Permission, OrderPermissions.ReadOwnDeliveryHistory),
                    new Claim(CustomClaimTypes.Permission, OrderPermissions.GetAvailableForDelivery),
                    new Claim(CustomClaimTypes.Permission, OrderPermissions.ChangeStatusUntilDelivered)
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
