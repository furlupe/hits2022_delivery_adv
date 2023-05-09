using DeliveryDeck_Backend_Final.Auth.BLL.Services;
using DeliveryDeck_Backend_Final.Auth.DAL;
using DeliveryDeck_Backend_Final.Auth.DAL.Entities;
using DeliveryDeck_Backend_Final.Auth.DAL.Extensions;
using DeliveryDeck_Backend_Final.Common.Enumerations;
using DeliveryDeck_Backend_Final.Common.Interfaces.Auth;
using DeliveryDeck_Backend_Final.Common.Middlewares;
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
                .AddDefaultTokenProviders()
                .AddUserManager<UserManager<AppUser>>()
                .AddRoleManager<RoleManager<Role>>();

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ITokenService, JwtTokenService>();
            builder.Services.AddScoped<IKeyProvider, RandomKeyProvider>();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.AddJwtAuthentification();
            builder.Services.AddAuthorization();
        }

        public static async Task<WebApplication> UseAuth(this WebApplication app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            await app.EnsureMigrated();
            await app.AddAuthRoles();
            await app.AddStaff();

            return app;
        }

        private static async Task<WebApplication> EnsureMigrated(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<AuthContext>();

            if ((await context.Database.GetPendingMigrationsAsync()).Any())
            {
                await context.Database.MigrateAsync();
            }

            return app;
        }
        private static async Task<WebApplication> AddAuthRoles(this WebApplication app)
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
            }

            return app;
        }

        private static async Task<WebApplication> AddStaff(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var userMgr = scope.ServiceProvider.GetService<UserManager<AppUser>>();

            if (await userMgr.FindByEmailAsync("admin@example.com") is not null)
            {
                return app;
            }

            var admin = new AppUser
            {
                FullName = "admin",
                BirthDate = DateTime.UtcNow,
                Gender = Gender.Female,
                Email = "admin@example.com"
            };

            var result = await userMgr.CreateAsync(admin, "_String1");
            if (!result.Succeeded) { return app; }
            await userMgr.AddToRoleAsync(admin, RoleType.Admin.ToString());

            return app;
        }
    }
}
