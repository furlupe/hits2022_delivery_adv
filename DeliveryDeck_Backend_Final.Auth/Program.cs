using DeliveryDeck_Backend_Final.Auth.BLL.Extensions;
using DeliveryDeck_Backend_Final.JWT.Classes;
using DeliveryDeck_Backend_Final.JWT.Extenions;

using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.AddAuth(builder.Configuration["AUTH_DB_CONNECTION"]!); // наш auth BLL

builder.UseJwtOptions(
    builder.Configuration["JWT_ISSUER"]!,
    builder.Configuration["JWT_AUDIENCE"]!,
    int.Parse(builder.Configuration["JWT_LIFETIME"]!),
    builder.Configuration["JWT_KEY"]!
    )
.AddJwtAuthentification(
    builder.Configuration["JWT_ISSUER"]!,
    builder.Configuration["JWT_AUDIENCE"]!,
    builder.Configuration["JWT_KEY"]!
    );

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

await app.UseAuth(); // вносим пользовательские роли

app.MapControllers();

app.Run();
