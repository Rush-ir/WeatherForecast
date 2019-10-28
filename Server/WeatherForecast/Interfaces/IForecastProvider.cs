using System.Threading.Tasks;
using WeatherForecast.Entities.Models;

namespace WeatherForecast.Interfaces
{
    /// <summary>
    /// Interface containing methods to retrieve forecast data from an external api
    /// </summary>
    public interface IForecastProvider
    {
        /// <summary>
        /// Retrieves forecast basing on city name and country code
        /// </summary>
        /// <param name="cityName"></param>
        /// <param name="countryCode"></param>
        /// <returns cref="ForecastModel"></returns>
        public Task<ForecastModel> GetForecastByCityName(string cityName, string countryCode);

        /// <summary>
        /// Retrieves forecast basing on zip code and country code
        /// </summary>
        /// <param name="zipCode"></param>
        /// <param name="countryCode"></param>
        /// <returns cref="ForecastModel"></returns>
        public Task<ForecastModel> GetForecastByZipCode(string zipCode, string countryCode);
    }
}
