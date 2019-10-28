using System.Threading.Tasks;
using WeatherForecast.Entities.Models;

namespace WeatherForecast.Interfaces
{
    /// <summary>
    /// Interface containing methods to retrieve weather data from an external api
    /// </summary>
    public interface IWeatherProvider
    {
        /// <summary>
        /// Retrieves current weather basing on city name and country code
        /// </summary>
        /// <param name="cityName"></param>
        /// <param name="countryCode"></param>
        /// <returns cref="CurrentWeatherModel"></returns>
        public Task<CurrentWeatherModel> GetCurrentWeatherByCityName(string cityName, string countryCode);

        /// <summary>
        /// Retrieves current weather basing on zip code and country code
        /// </summary>
        /// <param name="zipCode"></param>
        /// <param name="countryCode"></param>
        /// <returns cref="CurrentWeatherModel"></returns>
        public Task<CurrentWeatherModel> GetCurrentWeatherByZipCode(string zipCode, string countryCode);
    }
}
