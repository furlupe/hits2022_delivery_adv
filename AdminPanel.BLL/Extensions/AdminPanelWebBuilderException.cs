using AdminPanel.BLL.Mappers;
using AdminPanel.BLL.Services;
using DeliveryDeck_Backend_Final.Auth.DAL;
using DeliveryDeck_Backend_Final.Auth.DAL.Entities;
using DeliveryDeck_Backend_Final.Auth.DAL.Extensions;
using DeliveryDeck_Backend_Final.Backend.DAL.Extensions;
using DeliveryDeck_Backend_Final.Common.Interfaces.AdminPanel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace AdminPanel.BLL.Extensions
{
    public static class AdminPanelWebBuilderException
    {
        public static WebApplicationBuilder UseAdminComponent(this WebApplicationBuilder builder, string authConnectionString, string backendConnectionString)
        {
            builder.UseAuthDAL(authConnectionString);
            builder.UseBackendDAL(backendConnectionString);

            builder.Services.AddAutoMapper(typeof(DtoToEntityMapper));

            builder.Services.AddIdentity<AppUser, Role>()
               .AddEntityFrameworkStores<AuthContext>()
               .AddDefaultTokenProviders()
               .AddUserManager<UserManager<AppUser>>()
               .AddSignInManager<SignInManager<AppUser>>();

            builder.Services.AddScoped<IAdminRestaurantService, AdminRestaurantService>();
            builder.Services.AddScoped<IAdminUserService, AdminUserService>();
            builder.Services.AddScoped<IAdminAuthService, AdminAuthService>();

            return builder;
        }
    }
}
