using DeliveryDeck_Backend_Final.JWT.Classes;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryDeck_Backend_Final.JWT.Extenions
{
    public static class UseJwtOptionsExtension
    {
        public static WebApplicationBuilder UseJwtOptions(
            this WebApplicationBuilder builder,
            string issuer,
            string audience,
            int lifetime,
            string key)
        {
            builder.Services.AddOptions()
                .Configure<JwtConfig>(x => x.Issuer = issuer)
                .Configure<JwtConfig>(x => x.Audience = audience)
                .Configure<JwtConfig>(x => x.Lifetime = lifetime)
                .Configure<JwtConfig>(x => x.Key = key);

            return builder;
        }
    }
}
