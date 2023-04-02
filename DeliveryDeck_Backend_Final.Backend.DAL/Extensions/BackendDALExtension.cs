﻿using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryDeck_Backend_Final.Backend.DAL.Extensions
{
    public static class BackendDALExtension
    {
        public static void UseBackendDAL(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<BackendContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("BackendConnection")));
        }
    }
}
