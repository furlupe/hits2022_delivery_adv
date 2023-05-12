using DeliveryDeck_Backend_Final.JWT.Classes;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryDeck_Backend_Final.JWT.Extenions
{
    public static class UseJwtOptionsExtension
    {
        public static WebApplicationBuilder UseJwtOptions(this WebApplicationBuilder builder, IConfigurationSection section)
        {
            builder.Services.Configure<JwtConfig>(section);
            return builder;
        }
    }
}
