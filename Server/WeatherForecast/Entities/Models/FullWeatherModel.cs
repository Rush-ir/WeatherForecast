using System.Text.Json.Serialization;

namespace WeatherForecast.Entities.Models
{
    /// <summary>
    /// Full weather model (local) containing current weather and forecast
    /// </summary>
    public class FullWeatherModel
    {
        /// <summary>
        /// Forecast
        /// </summary>
        [JsonPropertyName("forecast")]
        public ForecastModel Forecast { get; set; }

        /// <summary>
        /// Current weather
        /// </summary>
        [JsonPropertyName("currentWeather")]
        public CurrentWeatherModel CurrentWeather { get; set; }
    }
}
