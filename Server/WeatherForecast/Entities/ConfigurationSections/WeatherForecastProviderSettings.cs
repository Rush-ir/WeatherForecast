using System;
using WeatherForecast.Entities.OpenWeatherApiEntities;

namespace WeatherForecast.Entities.ConfigurationSections
{
    /// <summary>
    /// Class containing settings for external api data provider 
    /// </summary>
    public class WeatherForecastProviderSettings
    {
        /// <summary>
        /// Provider name = external api name
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// External api base address 
        /// </summary>
        public string BaseAddress { get; set; }

        /// <summary>
        /// External api address to get icons
        /// </summary>
        public string IconStorageAddress { get; set; }

        /// <summary>
        /// Night icon key, which represents if current icon shows weather state at night, i.e. "n"
        /// </summary>
        public string NigthIconKey { get; set; }

        /// <summary>
        /// Daylight icon key, which represents if current icon shows weather state during the day, i.e. "n"
        /// </summary>
        public string DayIconKey { get; set; }

        /// <summary>
        /// Timespan to wait before request times out for http client to an external api
        /// </summary>
        public TimeSpan Timeout { get; set; }

        /// <summary>
        /// Specisies different units (i.e temprature: C, K, F) format
        /// Standard, metric, and imperial units are available
        /// </summary>
        public UnitsFormats UnitsFormat { get; set; }

        /// <summary>
        /// Part of request query to search by city name accordingly to an external api documentation
        /// </summary>
        public string SearchByCityParameter { get; set; }

        /// <summary>
        /// Part of request query to search by zip code accordingly to an external api documentation
        /// </summary>
        public string SearchByZipCodeParameter { get; set; }

        /// <summary>
        /// Represents a default country code which is used as a part of external api requests
        /// </summary>
        public string DefaultCountryCode { get; set; }
    }
}
