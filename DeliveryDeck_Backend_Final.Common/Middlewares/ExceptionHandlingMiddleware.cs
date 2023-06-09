﻿using DeliveryDeck_Backend_Final.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.TagHelpers;
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
                    ex.Message,
                    ex.Errors
                });
            }
            catch (BadHttpRequestException ex)
            {
                await WriteResponse(context, ex.StatusCode, new { ex.Message });
            }
            catch (RepositoryEntityNotFoundException ex)
            {
                await WriteResponse(context, StatusCodes.Status404NotFound, new
                {
                    ex.Message,
                    ex.Optional
                });
            }
            catch (RepositoryEntityAlreadyExistsException ex)
            {
                await WriteResponse(context, StatusCodes.Status409Conflict, new { ex.Message });
            }
            catch (Exception ex) when (ex is PersonUnemployedException || ex is OrderUnableToChangeStatusException)
            {
                await WriteResponse(context, StatusCodes.Status400BadRequest, new {ex.Message});
            }
            catch
            {
                // log here idk idc
                throw;
            }
        }

        private static async Task WriteResponse(HttpContext context, int statusCode, object? obj = null)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(obj));
        }
    }
}
