using DeliveryDeck_Backend_Final.Auth.BLL.Services;
using DeliveryDeck_Backend_Final.Auth.DAL;
using DeliveryDeck_Backend_Final.Auth.DAL.Entities;
using DeliveryDeck_Backend_Final.Auth.DAL.Extensions;
using DeliveryDeck_Backend_Final.Common.Enumerations;
using DeliveryDeck_Backend_Final.Common.Interfaces;
using DeliveryDeck_Backend_Final.Common.Middlewares;
using DeliveryDeck_Backend_Final.Common.Utils;
using DeliveryDeck_Backend_Final.JWT.Extenions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
            builder.Services.AddScoped<IUserService, UserService>();

            builder.AddJwtAuthentification();
            builder.Services.AddAuthorization();
        }

        public static async Task UseAuth(this WebApplication app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            await app.AddAuthRoles();
        }

        private static async Task AddAuthRoles(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var roleManager = scope.ServiceProvider.GetService<RoleManager<Role>>();

            foreach (RoleType role in Enum.GetValues(typeof(RoleType)))
            {

                if (!await roleManager.RoleExistsAsync(role.ToString()))
                {
                    var newRole = new Role
                    {
                        Name = role.ToString(),
                        Type = role
                    };

                    await roleManager.CreateAsync(newRole);
                }

                var existingRole = await roleManager.Roles.SingleAsync(r => r.Type == role);
                var existingClaims = await roleManager.GetClaimsAsync(existingRole);

                foreach (var claim in RoleClaims.Claims.First(kvp => kvp.Key == role).Value)
                {
                    if (existingClaims.Any(c => c.Value == claim.Value && c.Type == claim.Type)) continue;

                    await roleManager.AddClaimAsync(existingRole, claim);
                }
            }
        }
    }
}
