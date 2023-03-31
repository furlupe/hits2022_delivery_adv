using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using DeliveryDeck_Backend_Final.Common.Interfaces;
using DeliveryDeck_Backend_Final.Common.Enumerations;
using DeliveryDeck_Backend_Final.JWT.Extenions;
using DeliveryDeck_Backend_Final.Auth.DAL.Entities;
using DeliveryDeck_Backend_Final.Auth.DAL.Extensions;
using DeliveryDeck_Backend_Final.Auth.DAL;
using DeliveryDeck_Backend_Final.Auth.BLL.Services;

namespace DeliveryDeck_Backend_Final.Auth.BLL.Extensions
{
    public static class AuthWebBuilderExtension
    {
        public static void AddAuth(this WebApplicationBuilder builder)
        {
            builder.UseAuthDAL();

            builder.Services.AddIdentity<AppUser, Role>()
                .AddEntityFrameworkStores<AuthContext>()
                .AddUserManager<UserManager<AppUser>>()
                .AddRoleManager<RoleManager<Role>>();

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ITokenService, JwtTokenService>();
            builder.Services.AddScoped<IKeyProvider, RandomKeyProvider>();

            builder.AddJwtAuthentification();
        }

        public static async Task AddAuthRoles(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetService<RoleManager<Role>>();
                
                foreach(RoleType role in Enum.GetValues(typeof(RoleType)))
                {
                    if (await roleManager.RoleExistsAsync(role.ToString())) continue;

                    await roleManager.CreateAsync(new Role
                    {
                        Name = role.ToString(),
                        Type = role
                    });
                }
            }
        }
    }
}
