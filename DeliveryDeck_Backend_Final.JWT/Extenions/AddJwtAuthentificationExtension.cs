using DeliveryDeck_Backend_Final.JWT.Classes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryDeck_Backend_Final.JWT.Extenions
{
    public static class AddJwtAuthentificationExtension
    {
        public static void AddJwtAuthentification(this WebApplicationBuilder builder)
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
                    ValidIssuer = JwtConfigurations.Issuer,
                    ValidateAudience = true,
                    ValidAudience = JwtConfigurations.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = JwtConfigurations.GetSymmetricSecurityKey(),
                    ClockSkew = TimeSpan.Zero
                };
            });
        }
    }
}
