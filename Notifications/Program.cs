using DeliveryDeck_Backend_Final.JWT.Extenions;
using Notifications.BLL.Extensions;
using Notifications.BLL.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .SetIsOriginAllowed(_ => true);
    });
});

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

builder.AddNotificationsListener(builder.Configuration["RABBIT_CONNECTION"]!);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("/notifications");

app.Run();
