using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace WeatherForecast.Entities.Models
{
    /// <summary>
    /// Daily forecast model (local), which contains general forecast info including detailed 3-hourly forecast 
    /// </summary>
    public class DailyForecastModelOld
    {
        /// <summary>
        /// Date in timezone of the forecasted city in format DD MM
        /// </summary>
        [JsonPropertyName("date")]
        public string Date { get; set; }

        public WeatherConditionsModel AvgWeatherConditions {
            get 
            {
                return new WeatherConditionsModel
                {
                    Humidity = (int)CalculateAverage(DetailedDailyForecasts, i => i.Humidity, 1),
                    Pressure = (int)CalculateAverage(DetailedDailyForecasts, i => i.Pressure, 0),

            };
            } 
            set { } 
        }

        /// <summary>
        /// Calculated average humidity based on all available humidity values from detailed 3-hourly forecast
        /// </summary>
        [JsonPropertyName("avgHumidity")]
        public int AvgHumidity => (int)CalculateAverage(DetailedDailyForecasts, i => i.Humidity, 1);

        /// <summary>
        /// Calculated average pressure based on all available pressure values from detailed 3-hourly forecast
        /// </summary>
        [JsonPropertyName("avgPressure")]
        public int AvgPressure
        {
            get { return (int)CalculateAverage(DetailedDailyForecasts, i => i.Pressure, 0); }
        }

        /// <summary>
        /// Calculated average temperature based on all available temperature values from detailed 3-hourly forecast
        /// </summary>
        [JsonPropertyName("avgTemperature")]
        public int AvgTemperature => (int)CalculateAverage(DetailedDailyForecasts, i => i.Temperature, 0);

        /// <summary>
        /// Calculated average wind speed based on all available wind speeds from detailed 3-hourly forecast
        /// </summary>
        [JsonPropertyName("avgWindSpeed")]
        public double AvgWindSpeed => CalculateAverage(DetailedDailyForecasts, i => i.WindSpeed, 1);

        /// <summary>
        /// Represents user-friedly weather description, i.e. rain, scattered clouds etc
        /// Most often weather condition during the day
        /// </summary>
        [JsonPropertyName("weatherDescription")]
        public string WeatherDescription { get; set; }

        /// <summary>
        /// Icon url corresponding to the most often weather description
        /// </summary>
        [JsonPropertyName("avgWeatherIconUrl")]
        public string AvgIconUrl { get; set; }

        /// <summary>
        /// Represents 3-hourly forecast for this.Date day
        /// </summary>
        [JsonPropertyName("detailedDailyForecasts")]
        public IEnumerable<DetailedDailyForecastModel> DetailedDailyForecasts { get; set; }

        public DailyForecastModel()
        {
            DetailedDailyForecasts = new List<DetailedDailyForecastModel>();
        }

        #region private methods
        private double CalculateAverage(IEnumerable<DetailedDailyForecastModel> forecasts, Func<DetailedDailyForecastModel, float> func, int precision)
        {
            return forecasts.Count() == 0 
                ? 0 
                : Math.Round(forecasts.Average(func), precision);
        }
        #endregion
    }
}
