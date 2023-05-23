using DeliveryDeck_Backend_Final.Backend.BLL.Extensions;
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

builder.UseBackendComponent(builder.Configuration["BACKEND_DB_CONNECTION"]!);

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

await app.UseBackendComponent();

app.MapControllers();

app.Run();
