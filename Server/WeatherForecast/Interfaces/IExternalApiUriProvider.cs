using System;

namespace WeatherForecast.Interfaces
{
    /// <summary>
    /// Interface containing methods to generate appropriate uri for request
    /// </summary>
    public interface IExternalApiUriProvider
    {
        /// <summary>
        /// Generates uri to get forecast basing on city name and country code
        /// </summary>
        /// <param name="cityName"></param>
        /// <param name="countryCode"></param>
        /// <returns cref="Uri"></returns>
        Uri GetForecastByCityUri(string cityName, string countryCode);

        /// <summary>
        /// Generates uri to get forecast basing on zip code and country code
        /// </summary>
        /// <param name="zipCode"></param>
        /// <param name="countryCode"></param>
        /// <returns cref="Uri"></returns>
        Uri GetForecastByZipCodeUri(string zipCode, string countryCode);

        /// <summary>
        /// Generates uri to get current weather basing on city name and country code
        /// </summary>
        /// <param name="cityName"></param>
        /// <param name="countryCode"></param>
        /// <returns cref="Uri"></returns>
        Uri GetCurrentWeatherByCityUri(string cityName, string countryCode);

        /// <summary>
        /// Generates uri to get current weather basing on zip code and country code
        /// </summary>
        /// <param name="zipCode"></param>
        /// <param name="countryCode"></param>
        /// <returns cref="Uri"></returns>
        Uri GetCurrentWeatherByZipCodeUri(string zipCode, string countryCode);
    }
}
