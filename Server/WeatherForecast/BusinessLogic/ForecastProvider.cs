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
    /// Provides methods to get forecasts
    /// </summary>
    public class ForecastProvider: IForecastProvider
    {
        private readonly IApiDataRetriever<OpenWeatherForecast> _forecastApiRetriever;
        private readonly IExternalApiUriProvider _externalApiUriProvider;
        private readonly IExternalApiResponseConverter<OpenWeatherForecast, ForecastModel> _forecastResponseConverter;
        private readonly ILogger<IForecastProvider> _logger;

        private const string cacheForecastContext = "GetForecast";

        public ForecastProvider(
            ILogger<IForecastProvider> logger,
            IApiDataRetriever<OpenWeatherForecast> forecastApiRetriever,
            IExternalApiUriProvider uriProvider,
            IExternalApiResponseConverter<OpenWeatherForecast, ForecastModel> forecastResponseConverter)
        {
            _logger = logger;
            _forecastApiRetriever = forecastApiRetriever;
            _externalApiUriProvider = uriProvider;
            _forecastResponseConverter = forecastResponseConverter;
        }

        /// <summary>
        /// Retrieves multiple days forecast by city name
        /// </summary>
        /// <param name="cityName"></param>
        /// <param name="countryCode"></param>
        /// <returns cref="ForecastModel"></returns>
        public async Task<ForecastModel> GetForecastByCityName(string cityName, string countryCode)
        {
            try
            {
                if (String.IsNullOrEmpty(cityName) || String.IsNullOrEmpty(countryCode))
                {
                    throw new ArgumentNullException("Invalid input parameters.");
                }
                //returns uri to get 5*24 hours forecast by city name and country code
                Uri forecastRequestUri = _externalApiUriProvider.GetForecastByCityUri(cityName, countryCode);

                //get forecast from external api or cache
                var forecast = await _forecastApiRetriever.GetData(forecastRequestUri, cacheForecastContext);

                //convert result from external api to ForecastModel
                return _forecastResponseConverter.Convert(forecast);
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
        /// Retrieves multiple days forecast by zip code
        /// </summary>
        /// <param name="zipCode"></param>
        /// <param name="countryCode"></param>
        /// <returns cref="ForecastModel"></returns>
        public async Task<ForecastModel> GetForecastByZipCode(string zipCode, string countryCode)
        {
            try
            {
                if (String.IsNullOrEmpty(zipCode) || String.IsNullOrEmpty(countryCode))
                {
                    throw new ArgumentNullException("Invalid input parameters.");
                }
                //returns uri to get 5 days forecast by zip code and country code
                Uri forecastRequestUri = _externalApiUriProvider.GetForecastByZipCodeUri(zipCode, countryCode);

                //get forecast from external api or cache
                var forecast = await _forecastApiRetriever.GetData(forecastRequestUri, cacheForecastContext);

                //convert result from external api to ForecastModel
                return _forecastResponseConverter.Convert(forecast);
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
