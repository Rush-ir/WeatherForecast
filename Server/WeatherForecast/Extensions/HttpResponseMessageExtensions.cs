using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using WeatherForecast.Exceptions;
using System.Net.Http;
using System.Text.Json;
using WeatherForecast.Entities.OpenWeatherApiEntities.OpenWeatherJsonEntities;

namespace WeatherForecast.Extensions
{
    /// <summary>
    /// Extensions of HttpResponseMessage
    /// </summary>
    public static class HttpResponseMessageExtensions
    {
        /// <summary>
        /// Async method to check response status which throws custom HttpResponseWithStatusCodeException with status code and api error message
        /// </summary>
        /// <param name="response"></param>
        /// <returns cref="HttpResponseWithStatusCodeException">Exception with status code and api message</returns>
        public static async Task EnsureSuccessStatusCodeAsync(this HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            var content = await response.Content.ReadAsStringAsync();
            var deserializerContent = JsonSerializer.Deserialize<Error>(content, new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true,

            });

            throw new HttpResponseWithStatusCodeException(response.StatusCode, deserializerContent.Message);
        }
    }
}
