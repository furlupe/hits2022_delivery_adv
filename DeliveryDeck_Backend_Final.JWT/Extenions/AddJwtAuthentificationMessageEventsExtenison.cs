using DeliveryDeck_Backend_Final.JWT.Classes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DeliveryDeck_Backend_Final.JWT.Extenions
{
    public static class AddJwtAuthentificationMessageEventsExtenison
    {
        public static WebApplicationBuilder AddJwtAuthentificationWithMessageEvent(this WebApplicationBuilder builder, JwtConfig jwtConfig)
        {
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtConfig.Audience,
                    ValidateLifetime = true,
                    IssuerSigningKey = jwtConfig.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            (path.StartsWithSegments("/notifications")))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            return builder;
        }
    }
}
