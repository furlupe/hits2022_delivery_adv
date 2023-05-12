using DeliveryDeck_Backend_Final.Backend.BLL.Services;
using DeliveryDeck_Backend_Final.Backend.DAL;
using DeliveryDeck_Backend_Final.Backend.DAL.Extensions;
using DeliveryDeck_Backend_Final.Common.Interfaces.Backend;
using DeliveryDeck_Backend_Final.Common.Interfaces.RabbitMQ;
using DeliveryDeck_Backend_Final.Common.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryDeck_Backend_Final.Backend.BLL.Extensions
{
    public static class BackendWebBuilderExtension
    {
        public static WebApplicationBuilder UseBackendComponent(this WebApplicationBuilder builder)
        {
            builder.UseBackendDAL();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IRestaurantService, RestaurantService>();
            builder.Services.AddScoped<IDishService, DishService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IResourceAuthorizationService, ResourceAuthorizationService>();

            builder.Services.AddSingleton<IRabbitMqService, RabbitMqService>();

            return builder;
        }

        public async static Task<WebApplication> UseBackendComponent(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<BackendContext>();

                if ((await context.Database.GetPendingMigrationsAsync()).Any())
                {
                    await context.Database.MigrateAsync();
                }

                return app.UseExceptionMiddleware();
            }
        }

        public static WebApplication UseExceptionMiddleware(this WebApplication app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            return app;
        }
    }
}
