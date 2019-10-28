using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WeatherForecast.Exceptions;

namespace WeatherForecast.CustomMiddlewares
{
    /// <summary>
    /// Custom exception middleware
    /// Catches exception, logs them and returned formatted user-friendly response
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        /// <summary>
        /// Async method to hadle exceptions globally
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ex"></param>
        /// <returns>Status code and message of local api response or passed from external api</returns>
        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            HttpResponseWithStatusCodeException exceptionWithStatusCode = ex as HttpResponseWithStatusCodeException;

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exceptionWithStatusCode != null 
                ? (int)exceptionWithStatusCode.StatusCode 
                : (int)HttpStatusCode.InternalServerError;

            string apiMessage = exceptionWithStatusCode != null 
                ? exceptionWithStatusCode.ApiMessage 
                : "Internal Server Error.";
            
            return context.Response.WriteAsync(apiMessage);
        }
    }
}
