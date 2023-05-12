using DeliveryDeck_Backend_Final.JWT.Classes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryDeck_Backend_Final.JWT.Extenions
{
    public static class AddJwtAuthentificationExtension
    {
        public static WebApplicationBuilder AddJwtAuthentification(this WebApplicationBuilder builder, JwtConfig jwtConfig)
        {
            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtConfig.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = jwtConfig.GetSymmetricSecurityKey(),
                    ClockSkew = TimeSpan.Zero
                };
            });
            return builder;
        }
    }
}
