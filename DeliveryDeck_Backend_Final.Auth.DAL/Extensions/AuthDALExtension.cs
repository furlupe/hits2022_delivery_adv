using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryDeck_Backend_Final.Auth.DAL.Extensions
{
    public static class AuthDALExtension
    {
        public static void UseAuthDAL(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<AuthContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


        }
    }
}
