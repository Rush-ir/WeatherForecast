using Microsoft.AspNetCore.Builder;
using WeatherForecast.CustomMiddlewares;

namespace WeatherForecast.Extensions
{
    /// <summary>
    /// Exception middleware extensions
    /// </summary>
    public static class ExceptionMiddlewareExtensions
    {
        public static void UseExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
