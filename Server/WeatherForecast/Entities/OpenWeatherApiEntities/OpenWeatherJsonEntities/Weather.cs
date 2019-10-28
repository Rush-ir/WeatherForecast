using System.Collections.Generic;

namespace WeatherForecast.Entities.OpenWeatherApiEntities.OpenWeatherJsonEntities
{
    /// <summary>
    /// External api entity
    /// Represents weather conditions
    /// </summary>
    public class Weather
    {
        /// <summary>
        /// External api entity
        /// Collection of weather details
        /// </summary>
        public IEnumerable<WeatherDetails> Details { get; set; }
    }

    /// <summary>
    /// External api entity
    /// Represents weather details
    /// </summary>
    public class WeatherDetails
    {
        /// <summary>
        /// User-friednly weather description with details: LIGHT rain, SCATTERED clouds etc. 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Weather state name: rain, clouds etc. 
        /// </summary>
        public string Main { get; set; }

        /// <summary>
        /// Icon name, which shows weather description: rain, clouds etc. 
        /// </summary>
        public string Icon { get; set; }
    }
}
