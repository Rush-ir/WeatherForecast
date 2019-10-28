using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WeatherForecast.Entities.ConfigurationSections;
using WeatherForecast.Entities.Models;
using WeatherForecast.Interfaces;

namespace WeatherForecast.Controllers
{
    /// <summary>
    /// Controller which provides endpoints to get weather and forecast details
    /// </summary>
    [ApiController]
    [Route("api/weather/forecast")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IForecastProvider _forecastProvider;
        private readonly IWeatherProvider _weatherProvider;
        private readonly WeatherForecastProviderSettings _weatherForecastProviderSettings;

        public WeatherForecastController(
            IForecastProvider forecastProvider,
            IWeatherProvider weatherProvider,
            IOptions<WeatherForecastProviderSettings> weatherForecastProviderSettingsAccessor)
        {
            _forecastProvider = forecastProvider;
            _weatherProvider = weatherProvider;
            _weatherForecastProviderSettings = weatherForecastProviderSettingsAccessor.Value;
        }

        /// <summary>
        /// Async method to get complete weather forecast by city name and country code
        /// </summary>
        /// <param name="city"></param>
        /// <param name="countryCode">Optional</param>
        /// <returns cref="FullWeatherModel"></returns>
        // GET: https://localhost:44313/api/weather/forecast/city/london?countrycode=uk
        [HttpGet]
        [Route("[action]/{city}")]
        public async Task<ActionResult<FullWeatherModel>> City(string city, string countryCode)
        {
            //if country code is not specified, then use default one from appsettings.json
            //by default it is DE (Germany)
            if (string.IsNullOrEmpty(countryCode))
            {
                countryCode = _weatherForecastProviderSettings.DefaultCountryCode;
            }

            var forecast = await _forecastProvider.GetForecastByCityName(city, countryCode);
            var currentWeather = await _weatherProvider.GetCurrentWeatherByCityName(city, countryCode);

            return Ok(new FullWeatherModel { Forecast = forecast, CurrentWeather = currentWeather });
        }

        /// <summary>
        /// Async method to get complete weather forecast by zip code and country code
        /// </summary>
        /// <param name="zipCode"></param>
        /// <param name="countryCode">Optional</param>
        /// <returns cref="FullWeatherModel"></returns>
        // GET: https://localhost:44313/api/weather/forecast/zipcode/94040?countrycode=us
        [HttpGet]
        [Route("[action]/{zipCode}")]
        public async Task<ActionResult<FullWeatherModel>> ZipCode(string zipCode, string countryCode)
        {
            //if country code is not specified, then use default one from appsettings.json
            //by default it is DE (Germany)
            if (string.IsNullOrEmpty(countryCode))
            {
                countryCode = _weatherForecastProviderSettings.DefaultCountryCode;
            }
            var forecast = await _forecastProvider.GetForecastByZipCode(zipCode, countryCode);
            var currentWeather = await _weatherProvider.GetCurrentWeatherByZipCode(zipCode, countryCode);

            return Ok(new FullWeatherModel { Forecast = forecast, CurrentWeather = currentWeather });
        }

    }
}
