﻿using DeliveryDeck_Backend_Final.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace DeliveryDeck_Backend_Final.Common.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (IdentityException ex)
            {
                await WriteResponse(context, ex.StatusCode, new
                {
                    Message = ex.Message,
                    Errors = ex.Errors
                });
            }
            catch (BadHttpRequestException ex)
            {
                await WriteResponse(context, ex.StatusCode, new { Message = ex.Message });
            }
            /*catch
            {
                await WriteResponse(context, StatusCodes.Status500InternalServerError, new { Message = "go outside" });
            }*/
        }

        private static async Task WriteResponse(HttpContext context, int statusCode, object? obj = null)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(obj));
        }
    }
}