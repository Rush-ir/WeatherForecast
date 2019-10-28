using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using WeatherForecast.ApiResponseConverters.DataConverters;
using WeatherForecast.Entities.ConfigurationSections;
using WeatherForecast.Entities.Models;
using WeatherForecast.Entities.OpenWeatherApiEntities.OpenWeatherJsonEntities;
using WeatherForecast.Extensions;
using WeatherForecast.Interfaces;

namespace WeatherForecast.ApiResponseConverters
{
    /// <summary>
    /// Entities converter: from external api entity (OpenWeather) to local model (CurrentWeatherModel)
    /// </summary>
    public class WeatherResponseConverter : IExternalApiResponseConverter<OpenWeather, CurrentWeatherModel>
    {
        private readonly WeatherForecastProviderSettings _wfProviderSettings;

        private readonly ILogger<WeatherResponseConverter> _logger;

        public WeatherResponseConverter(
            ILogger<WeatherResponseConverter> logger,
            IOptions<WeatherForecastProviderSettings> weatherForecastProviderSettingsAccessor)
        {
            _logger = logger;
            _wfProviderSettings = weatherForecastProviderSettingsAccessor.Value;
        }

        /// <summary>
        /// Converts entities: OpenWeather to CurrentWeatherModel
        /// </summary>
        /// <param name="externalApiWeather"></param>
        /// <returns cref="CurrentWeatherModel"></returns>
        public CurrentWeatherModel Convert(OpenWeather externalApiWeather)
        {
            try
            {
                if (externalApiWeather == null 
                    || externalApiWeather.Weather == null
                    || externalApiWeather.Wind == null
                    || externalApiWeather.Main == null)
                {
                    throw new ArgumentNullException("Invalid input parameter.");
                }
                var openWeatherEntity = externalApiWeather.Weather.FirstOrDefault();

                CurrentWeatherModel currentWeather = new CurrentWeatherModel
                {
                    Date = externalApiWeather.Dt.ToDateTime().ToWeatherDateTimeFormat(externalApiWeather.Timezone),
                    WeatherConditions = new WeatherConditionsModel
                    {
                        TemperatureFormat = WeatherUnitsFormatsConverter.GetTemperatureFormat(_wfProviderSettings.UnitsFormat),
                        Humidity = externalApiWeather.Main.Humidity,
                        Pressure = externalApiWeather.Main.Pressure,
                        Temperature = (int)Math.Round(externalApiWeather.Main.Temp, 0),
                        WeatherDescription = openWeatherEntity.Description,
                        IconUrl = IconConverter.Generate(_wfProviderSettings.IconStorageAddress, openWeatherEntity.Icon),
                        WindSpeed = Math.Round(externalApiWeather.Wind.Speed, 1)
                    }

                };

                return currentWeather;
            }
            catch (Exception ex)
            {
                string errorMessage = "Something went wrond during convertion from OpenWeather to CurrentWeatherModel.";
                _logger.LogError(errorMessage, ex);
                throw new Exception(errorMessage, ex);
            }
        }
    }
}
