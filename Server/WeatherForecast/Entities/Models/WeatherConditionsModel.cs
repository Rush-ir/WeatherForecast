using System.Text.Json.Serialization;
using WeatherForecast.Entities.Enums;

namespace WeatherForecast.Entities.Models
{
    /// <summary>
    /// Model (local) representing common weather conditions
    /// </summary>
    public class WeatherConditionsModel
    {
        /// <summary>
        /// Temperature
        /// </summary>
        [JsonPropertyName("temperature")]
        public int Temperature { get; set; }

        /// <summary>
        /// Temperature format: kelvin (K), farenhgeit (F), celcius (C)
        /// </summary>
        [JsonPropertyName("temperatureFormat")]
        public string TemperatureFormat { get; set; }

        /// <summary>
        /// Url of icon representing weather conditions, i.e. sunny, rain etc.
        /// </summary>
        [JsonPropertyName("weatherIconUrl")]
        public string IconUrl { get; set; }

        /// <summary>
        /// Represents user-friedly weather description with details, i.e. OVERCAST clouds, clear SKY etc.
        /// </summary>
        [JsonPropertyName("weatherDescription")]
        public string WeatherDescription { get; set; }

        /// <summary>
        /// Humidity
        /// </summary>
        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }

        /// <summary>
        /// Pressure
        /// </summary>
        [JsonPropertyName("pressure")]
        public int Pressure { get; set; }

        /// <summary>
        /// Wind speed
        /// </summary>
        [JsonPropertyName("windSpeed")]
        public double WindSpeed { get; set; }

        public override bool Equals(object obj)
        {
            var weatherConditionsObject = obj as WeatherConditionsModel;
            if (weatherConditionsObject == null)
            {
                return base.Equals(obj);
            }
            else
            {
                return Humidity == weatherConditionsObject.Humidity
                    && IconUrl == weatherConditionsObject.IconUrl
                    && Pressure == weatherConditionsObject.Pressure
                    && Temperature == weatherConditionsObject.Temperature
                    && TemperatureFormat == weatherConditionsObject.TemperatureFormat
                    && WeatherDescription == weatherConditionsObject.WeatherDescription
                    && WindSpeed == weatherConditionsObject.WindSpeed;
            }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Humidity.GetHashCode();
                hash = hash * 23 + IconUrl.GetHashCode();
                hash = hash * 23 + Pressure.GetHashCode();
                hash = hash * 23 + Temperature.GetHashCode();
                hash = hash * 23 + TemperatureFormat.GetHashCode();
                hash = hash * 23 + WeatherDescription.GetHashCode();
                hash = hash * 23 + WindSpeed.GetHashCode();
                return hash;
            }
        }
    }
}
