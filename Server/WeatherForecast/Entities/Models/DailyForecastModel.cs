using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace WeatherForecast.Entities.Models
{
    /// <summary>
    /// Daily forecast model (local), which contains general forecast info including detailed 3-hourly forecast 
    /// </summary>
    public class DailyForecastModel
    {
        /// <summary>
        /// Date in timezone of the forecasted city in format DD MM
        /// </summary>
        [JsonPropertyName("date")]
        public string Date { get; set; }

        /// <summary>
        /// Contains average values of weather conditions based on all available values from detailed 3-hourly forecast
        /// </summary>
        public WeatherConditionsModel AvgWeatherConditions { get; set; }

        /// <summary>
        /// Represents 3-hourly forecast for this.Date day
        /// </summary>
        [JsonPropertyName("detailedDailyForecasts")]
        public IList<DetailedDailyForecastModel> DetailedDailyForecasts { get; set; }

        public DailyForecastModel()
        {
            DetailedDailyForecasts = new List<DetailedDailyForecastModel>();
        }

        public override bool Equals(object obj)
        {
            var dailyForecastObject = obj as DailyForecastModel;
            if (dailyForecastObject == null)
            {
                return base.Equals(obj);
            }
            else
            {
                bool equalDetailedDailyForecasts = false;

                if (DetailedDailyForecasts.Count == dailyForecastObject.DetailedDailyForecasts.Count)
                {
                    for (int i = 0; i < DetailedDailyForecasts.Count; i++)
                    {
                        if (!DetailedDailyForecasts[i].Equals(dailyForecastObject.DetailedDailyForecasts[i]))
                        {
                            return false;
                        }
                    }
                    equalDetailedDailyForecasts = true;
                }

                return AvgWeatherConditions.Equals(dailyForecastObject.AvgWeatherConditions)
                    && Date == dailyForecastObject.Date
                    && equalDetailedDailyForecasts;
            }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + AvgWeatherConditions.GetHashCode();
                hash = hash * 23 + Date.GetHashCode();
                hash = hash * 23 + DetailedDailyForecasts.GetHashCode();
                return hash;
            }
        }
    }
}
