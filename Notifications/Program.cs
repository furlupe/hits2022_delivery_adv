using DeliveryDeck_Backend_Final.JWT.Classes;
using DeliveryDeck_Backend_Final.JWT.Extenions;
using Microsoft.AspNetCore.SignalR;
using Notifications.Hubs;
using Notifications.Listeners;
using Notifications.Providers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserIdProvider, NameUserIdProvider>();

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

builder.UseJwtOptions(builder.Configuration.GetSection("jwt"))
    .AddJwtAuthentificationWithMessageEvent(builder.Configuration.GetSection("jwt").Get<JwtConfig>()!);
builder.Services.AddAuthorization();


builder.Services.AddHostedService<RabbitListener>();


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
