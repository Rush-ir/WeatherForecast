using System.Collections.Generic;
using System.Text.Json.Serialization;
using WeatherForecast.Entities.Enums;

namespace WeatherForecast.Entities.Models
{
    /// <summary>
    /// Forecast model (local)
    /// </summary>
    public class ForecastModel
    {
        /// <summary>
        /// City name
        /// </summary>
        [JsonPropertyName("city")]
        public string City { get; set; }

        /// <summary>
        /// Collection of daily forecasts
        /// </summary>
        [JsonPropertyName("dailyForecasts")]
        public IList<DailyForecastModel> DailyForecasts { get; set; }

        public ForecastModel(string city): this()
        {
            City = city;
        }

        public ForecastModel()
        {
            DailyForecasts = new List<DailyForecastModel>();
        }

        public override bool Equals(object obj)
        {
            var forecastObject = obj as ForecastModel;
            if (forecastObject == null)
            {
                return base.Equals(obj);
            }
            else
            {
                bool equalDailyForecasts = false;

                if (DailyForecasts.Count == forecastObject.DailyForecasts.Count)
                {
                    for (int i = 0; i < DailyForecasts.Count; i++)
                    {
                        if (!DailyForecasts[i].Equals(forecastObject.DailyForecasts[i]))
                        {
                            return false;
                        }
                    }
                    equalDailyForecasts = true;
                }                

                return City == forecastObject.City && equalDailyForecasts;
            }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + City.GetHashCode();
                hash = hash * 23 + DailyForecasts.GetHashCode();
                return hash;
            }
        }
    }
}
