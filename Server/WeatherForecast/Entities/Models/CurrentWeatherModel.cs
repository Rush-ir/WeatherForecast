using System;
using System.Text.Json.Serialization;
using WeatherForecast.Entities.Enums;

namespace WeatherForecast.Entities.Models
{
    /// <summary>
    /// Current weather model (local) contains details of current weather conditions
    /// </summary>
    public class CurrentWeatherModel
    {
        /// <summary>
        /// Date in timezone of the forecasted city
        /// </summary>
        [JsonPropertyName("date")]
        public string Date { get; set; }

        /// <summary>
        /// Weather conditions (temperature, pressure, humidity, wind speed etc.)
        /// </summary>
        [JsonPropertyName("conditions")]
        public WeatherConditionsModel WeatherConditions { get; set; }

        public override bool Equals(object obj)
        {
            var currentWeatherObject = obj as CurrentWeatherModel;
            if (currentWeatherObject == null)
            {
                return base.Equals(obj);
            }
            else
            {
                return Date == currentWeatherObject.Date
                    && WeatherConditions.Equals(currentWeatherObject.WeatherConditions);
            }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Date.GetHashCode();
                hash = hash * 23 + WeatherConditions.GetHashCode();
                return hash;
            }
        }
    }
}
