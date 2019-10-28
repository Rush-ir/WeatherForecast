using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using WeatherForecast.Entities.Models;
using WeatherForecast.Entities.OpenWeatherApiEntities.OpenWeatherJsonEntities;
using WeatherForecast.Exceptions;
using WeatherForecast.Interfaces;
using WeatherForecast.JsonEntities.OpenWeatherForecast;

namespace WeatherForecast.BusinessLogic
{
    /// <summary>
    /// Provides methods to get weather data
    /// </summary>
    public class WeatherProvider: IWeatherProvider
    {
        private readonly IApiDataRetriever<OpenWeather> _weatherApiRetriever;
        private readonly IExternalApiUriProvider _externalApiUriProvider;
        private readonly IExternalApiResponseConverter<OpenWeather, CurrentWeatherModel> _weatherResponseConverter;
        private readonly ILogger<IWeatherProvider> _logger;

        private const string cacheWeatherContext = "GetWeather";

        public WeatherProvider(
            ILogger<IWeatherProvider> logger,
            IApiDataRetriever<OpenWeather> weatherApiRetriever,
            IExternalApiUriProvider uriProvider,
            IExternalApiResponseConverter<OpenWeather, CurrentWeatherModel> weatherResponseConverter)
        {
            _logger = logger;
            _weatherApiRetriever = weatherApiRetriever;
            _externalApiUriProvider = uriProvider;
            _weatherResponseConverter = weatherResponseConverter;
        }

        /// <summary>
        /// Retrieves current weather by city name
        /// </summary>
        /// <param name="cityName"></param>
        /// <param name="countryCode"></param>
        /// <returns cref="CurrentWeatherModel"></returns>
        public async Task<CurrentWeatherModel> GetCurrentWeatherByCityName(string cityName, string countryCode)
        {
            try
            {
                if (String.IsNullOrEmpty(cityName) || String.IsNullOrEmpty(countryCode))
                {
                    throw new ArgumentNullException("Invalid input parameters."); 
                }
                //generate uri to get current weather by city name and country code
                Uri weatherRequestUri = _externalApiUriProvider.GetCurrentWeatherByCityUri(cityName, countryCode);

                //get weather data from external api or cache
                var weather = await _weatherApiRetriever.GetData(weatherRequestUri, cacheWeatherContext);

                //convert result from external api to CurrentWeatherModel
                return _weatherResponseConverter.Convert(weather);
            }
            catch (HttpResponseWithStatusCodeException ex)
            {
                _logger.LogError($"ApiMessage:{ex.ApiMessage}", ex);
                throw ex;
            }
            catch (Exception ex)
            {
                _logger.LogError("Something went wrong during getting data from external api.", ex);
                throw new HttpResponseWithStatusCodeException(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves current weather by zip code
        /// </summary>
        /// <param name="zipCode"></param>
        /// <param name="countryCode"></param>
        /// <returns cref="CurrentWeatherModel"></returns>
        public async Task<CurrentWeatherModel> GetCurrentWeatherByZipCode(string zipCode, string countryCode)
        {
            try
            {
                if (String.IsNullOrEmpty(zipCode) || String.IsNullOrEmpty(countryCode))
                {
                    throw new ArgumentNullException("Invalid input parameters.");
                }
                //returns uri to get current weather by zip code and country code
                Uri weatherRequestUri = _externalApiUriProvider.GetCurrentWeatherByZipCodeUri(zipCode, countryCode);

                //get weather data from external api or cache
                var weather = await _weatherApiRetriever.GetData(weatherRequestUri, cacheWeatherContext);

                //convert result from external api to CurrentWeatherModel
                return _weatherResponseConverter.Convert(weather);
            }
            catch (HttpResponseWithStatusCodeException ex)
            {
                _logger.LogError($"ApiMessage:{ex.ApiMessage}", ex);
                throw ex;
            }
            catch (Exception ex)
            {
                _logger.LogError("Something went wrong during getting data from external api.", ex);
                throw new HttpResponseWithStatusCodeException(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
