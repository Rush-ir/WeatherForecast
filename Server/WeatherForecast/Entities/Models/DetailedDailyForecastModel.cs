using System;
using System.Text.Json.Serialization;

namespace WeatherForecast.Entities.Models
{
    /// <summary>
    /// Detalized daily forecast model (local)
    /// </summary>
    public class DetailedDailyForecastModel
    {
        /// <summary>
        /// Time in timezone of the forecasted city in format HH:mm
        /// </summary>
        [JsonPropertyName("time")]
        public string Time { get; set; }

        /// <summary>
        /// Weather conditions (temperature, pressure, humidity, wind speed etc.)
        /// </summary>
        [JsonPropertyName("conditions")]
        public WeatherConditionsModel WeatherConditions { get; set; }

        public override bool Equals(object obj)
        {
            var detailedDailyForecastObject = obj as DetailedDailyForecastModel;
            if (detailedDailyForecastObject == null)
            {
                return base.Equals(obj);
            }
            else
            {
                return Time == detailedDailyForecastObject.Time
                    && WeatherConditions.Equals(detailedDailyForecastObject.WeatherConditions);
            }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Time.GetHashCode();
                hash = hash * 23 + WeatherConditions.GetHashCode();
                return hash;
            }
        }
    }
}
