﻿using Microsoft.AspNetCore.Http;
using Mubbi.Marketplace.API.Models;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.API.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch(Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = ex switch
            {
                ArgumentException a => StatusCodes.Status400BadRequest,
                DomainException d => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            var result = JsonConvert.SerializeObject(new ApiResponse<List<string>>(false, "The request finished on error.", ex.GetErrorList()));

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = code;
            await context.Response.WriteAsync(result);
        }
    }
}
