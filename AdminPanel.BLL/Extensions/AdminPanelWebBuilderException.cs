using AdminPanel.BLL.Services;
using DeliveryDeck_Backend_Final.Auth.DAL.Extensions;
using DeliveryDeck_Backend_Final.Backend.DAL.Extensions;
using DeliveryDeck_Backend_Final.Common.Interfaces.AdminPanel;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AdminPanel.BLL.Extensions
{
    public static class AdminPanelWebBuilderException
    {
        public static WebApplicationBuilder? UseAdminComponent(this WebApplicationBuilder? builder)
        {
            builder.UseAuthDAL();
            builder.UseBackendDAL();

            builder.Services.AddScoped<IAdminRestaurantService, AdminRestaurantService>();

            return builder;
        }
    }
}
