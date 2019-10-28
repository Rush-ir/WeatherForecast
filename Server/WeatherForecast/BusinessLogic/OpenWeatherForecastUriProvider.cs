using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using WeatherForecast.Entities.ConfigurationSections;
using WeatherForecast.Interfaces;

namespace WeatherForecast.BusinessLogic
{
    /// <summary>
    /// Contains methods to generate appropriate uri for request
    /// </summary>
    public class OpenWeatherForecastUriProvider: IExternalApiUriProvider
    {
        private readonly WeatherForecastProviderSettings _weatherForecastProviderSettings;
        private readonly ILogger<OpenWeatherForecastUriProvider> _logger;

        public OpenWeatherForecastUriProvider(
            ILogger<OpenWeatherForecastUriProvider> logger,            
            IOptions<WeatherForecastProviderSettings> weatherForecastProviderSettingsAccessor)
        {
            _logger = logger;            
            _weatherForecastProviderSettings = weatherForecastProviderSettingsAccessor.Value;
        }

        /// <summary>
        /// Generates uri to get current weather basing on city name and country code
        /// </summary>
        /// <param name="cityName"></param>
        /// <param name="countryCode"></param>
        /// <returns cref="Uri"></returns>
        public Uri GetCurrentWeatherByCityUri(string cityName, string countryCode)
        {
            if (String.IsNullOrEmpty(cityName)
                      || String.IsNullOrEmpty(countryCode))
            {
                throw new ArgumentNullException("Invalid input parameters.");
            }
            string endpoint = $"weather?{_weatherForecastProviderSettings.SearchByCityParameter}" +
                    $"={cityName},{countryCode}" +
                    $"&units={_weatherForecastProviderSettings.UnitsFormat}" +
                    $"&appid={_weatherForecastProviderSettings.ApiKey}";

            return new Uri(_weatherForecastProviderSettings.BaseAddress + endpoint);
        }

        /// <summary>
        /// Generates uri to get current weather basing on zip code and country code
        /// </summary>
        /// <param name="zipCode"></param>
        /// <param name="countryCode"></param>
        /// <returns cref="Uri"></returns>
        public Uri GetCurrentWeatherByZipCodeUri(string zipCode, string countryCode)
        {
            if (String.IsNullOrEmpty(zipCode)
                   || String.IsNullOrEmpty(countryCode))
            {
                throw new ArgumentNullException("Invalid input parameters.");
            }
            string endpoint = $"weather?{_weatherForecastProviderSettings.SearchByZipCodeParameter}" +
                    $"={zipCode},{countryCode}" +
                    $"&units={_weatherForecastProviderSettings.UnitsFormat}" +
                    $"&appid={_weatherForecastProviderSettings.ApiKey}";

            return new Uri(_weatherForecastProviderSettings.BaseAddress + endpoint);
        }

        /// <summary>
        /// Generates uri to get forecast basing on city name and country code
        /// </summary>
        /// <param name="cityName"></param>
        /// <param name="countryCode"></param>
        /// <returns cref="Uri"></returns>
        public Uri GetForecastByCityUri(string cityName, string countryCode)
        {
            if (String.IsNullOrEmpty(cityName)
                || String.IsNullOrEmpty(countryCode))
            {
                throw new ArgumentNullException("Invalid input parameters.");
            }
            string endpoint =  $"forecast?{_weatherForecastProviderSettings.SearchByCityParameter}" +
                    $"={cityName},{countryCode}" +
                    $"&units={_weatherForecastProviderSettings.UnitsFormat}" +
                    $"&appid={_weatherForecastProviderSettings.ApiKey}";

            return new Uri(_weatherForecastProviderSettings.BaseAddress + endpoint);
        }

        /// <summary>
        /// Generates uri to get forecast basing on zip code and country code
        /// </summary>
        /// <param name="zipCode"></param>
        /// <param name="countryCode"></param>
        /// <returns cref="Uri"></returns>
        public Uri GetForecastByZipCodeUri(string zipCode, string countryCode)
        {
            if (String.IsNullOrEmpty(zipCode) 
                || String.IsNullOrEmpty(countryCode))
            {
                throw new ArgumentNullException("Invalid input parameters.");
            }

            string endpoint = $"forecast?{_weatherForecastProviderSettings.SearchByZipCodeParameter}" +
                    $"={zipCode},{countryCode}" +
                    $"&units={_weatherForecastProviderSettings.UnitsFormat}" +
                    $"&appid={_weatherForecastProviderSettings.ApiKey}";

            return new Uri(_weatherForecastProviderSettings.BaseAddress + endpoint);
        }
    }
}
