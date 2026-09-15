using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.OpenApi.Exceptions;
using server.Errors;

namespace server.Middleware
{
    public class ExceptionMiddleware(RequestDelegate next , ILogger<ExceptionMiddleware> logger ,IHostEnvironment env )
    {
        // method to invokec middleware
        public async Task IvvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            // this will catch the unhandled exceptions 
            catch( Exception ex)
            {
                // logger for console
                logger.LogError(ex, "{}",ex.Message);
                // setting up the content type on context
                context.Response.ContentType = "application/json";
                // default status code, this is hardcoded status code 500 internal error
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                // logic to modify the response if it is developement mode
                var response = env.IsDevelopment()
                // if it is in development mode , include the stack trace with the log
                    ? new ApiException(context.Response.StatusCode ,ex.Message, ex.StackTrace )
                    : new ApiException(context.Response.StatusCode,ex.Message, "Internal Server error");


                // logic to serialize the response
                
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var json = JsonSerializer.Serialize(response,options);
                await context.Response.WriteAsync(json);




            }
        }
    }
}