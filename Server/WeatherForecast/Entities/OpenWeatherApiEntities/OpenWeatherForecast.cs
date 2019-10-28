using System.Collections.Generic;
using WeatherForecast.Entities.OpenWeatherApiEntities.OpenWeatherJsonEntities;

namespace WeatherForecast.JsonEntities.OpenWeatherForecast
{
    /// <summary>
    /// External api entity
    /// Open weather forecast representing forecast data structure from openweather provider
    /// </summary>
    public class OpenWeatherForecast
    {
        /// <summary>
        /// External api entity
        /// Represents data about city
        /// </summary>
        public City City { get; set; }

        /// <summary>
        /// External api entity
        /// Collection of weather details
        /// </summary>
        public IEnumerable<OpenWeather> List {get; set;}
    }
}
