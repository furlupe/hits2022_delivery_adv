using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryDeck_Backend_Final.Auth.DAL.Extensions
{
    public static class AuthDALExtension
    {
        public static void UseAuthDAL(this WebApplicationBuilder builder, string connectionString)
        {
            builder.Services.AddDbContext<AuthContext>(options =>
                options.UseNpgsql(connectionString));
        }
    }
}
