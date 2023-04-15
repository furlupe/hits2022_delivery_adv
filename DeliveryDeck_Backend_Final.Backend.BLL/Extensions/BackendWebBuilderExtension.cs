using DeliveryDeck_Backend_Final.Backend.BLL.Services;
using DeliveryDeck_Backend_Final.Backend.DAL.Extensions;
using DeliveryDeck_Backend_Final.Common.Interfaces.Backend;
using DeliveryDeck_Backend_Final.JWT.Extenions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryDeck_Backend_Final.Backend.BLL.Extensions
{
    public static class BackendWebBuilderExtension
    {
        public static void UseBackendComponent(this WebApplicationBuilder builder)
        {
            builder.UseBackendDAL();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IRestaurantService, RestaurantService>();
            builder.Services.AddScoped<IDishService, DishService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IResourceAuthorizationService, ResourceAuthorizationService>();

            builder.AddJwtAuthentification();
        }
    }
}
