using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using WeatherForecast.ApiResponseConverters.DataConverters;
using WeatherForecast.Entities.ConfigurationSections;
using WeatherForecast.Entities.Models;
using WeatherForecast.Entities.OpenWeatherApiEntities.OpenWeatherJsonEntities;
using WeatherForecast.Extensions;
using WeatherForecast.Interfaces;
using WeatherForecast.JsonEntities.OpenWeatherForecast;

namespace WeatherForecast.ApiResponseConverters
{
    /// <summary>
    /// Entities converter: from external api entity (OpenWeatherForecast) to local model (ForecastModel)
    /// </summary>
    public class ForecastResponseConverter : IExternalApiResponseConverter<OpenWeatherForecast, ForecastModel>
    {
        private readonly WeatherForecastProviderSettings _wfProviderSettings;
        private readonly ILogger<ForecastResponseConverter> _logger;

        public ForecastResponseConverter(
            ILogger<ForecastResponseConverter> logger,
            IOptions<WeatherForecastProviderSettings> weatherForecastProviderSettingsAccessor)
        {
            _logger = logger;
            _wfProviderSettings = weatherForecastProviderSettingsAccessor.Value;
        }

        /// <summary>
        /// Converts entities: OpenWeatherForecast to ForecastModel
        /// </summary>
        /// <param name="externalApiForecast"></param>
        /// <returns cref="ForecastModel"></returns>
        public ForecastModel Convert(OpenWeatherForecast externalApiForecast)
        {
            try 
            {
                if (externalApiForecast == null 
                    || externalApiForecast.List == null
                    || externalApiForecast.City == null)
                {
                    throw new ArgumentNullException("Invalid input parameter.");
                }

                ForecastModel forecast = new ForecastModel
                {
                    City = externalApiForecast.City.Name
                };

                List<OpenWeather> weatherList = externalApiForecast.List.ToList<OpenWeather>();

                if (weatherList.Count() == 0)
                {
                    throw new Exception("External api forecast is empty.");
                }

                string temperatureFormat = WeatherUnitsFormatsConverter.GetTemperatureFormat(_wfProviderSettings.UnitsFormat);
                string nigthIconKey = _wfProviderSettings.NigthIconKey;
                string dayIconKey = _wfProviderSettings.DayIconKey;

                DateTime currentDay = weatherList[0].Dt.ToDateTime();
                DateTime nextDay = currentDay;
                List<DetailedDailyForecastModel> detailedDailyForecasts = new List<DetailedDailyForecastModel>();

                //contains icons' names to define most often in a day to set AvgWeatherConditions.IconUrl
                //contains weather states to define most often in a day to set AvgWeatherConditions.WeatherDescription
                List<WeatherDetails> weatherDetailsForAvg = new List<WeatherDetails>();

                bool isLastDay = false;

                for (int i = 0; i < weatherList.Count(); i++)
                {
                    var item = weatherList[i];

                    var weatherDetails = item.Weather.FirstOrDefault();
                    weatherDetailsForAvg.Add(weatherDetails);

                    detailedDailyForecasts.Add(new DetailedDailyForecastModel
                    {
                        Time = nextDay.ToWeatherTimeFormat(item.Timezone),
                        WeatherConditions = new WeatherConditionsModel
                        {
                            IconUrl = IconConverter.Generate(_wfProviderSettings.IconStorageAddress, weatherDetails.Icon),
                            Humidity = item.Main.Humidity,
                            Pressure = item.Main.Pressure,
                            Temperature = (int)Math.Round(item.Main.Temp),
                            WindSpeed = Math.Round(item.Wind.Speed, 1),
                            TemperatureFormat = temperatureFormat,
                            WeatherDescription = weatherDetails.Description
                        }
                    });

                    if (i + 1 < weatherList.Count()) 
                    { 
                        nextDay = weatherList[i + 1].Dt.ToDateTime();
                    }
                    else
                    {
                        isLastDay = true;
                    }

                    if (currentDay.Date != nextDay.Date || isLastDay)
                    {
                        //for all days avg weather icon is the most often icon during the day
                        var mostOftenWeatherDescription = weatherDetailsForAvg.GroupBy(i => i.Main)
                            .OrderByDescending(i => i.Count())
                            .First();
                        var mostOftenIcon = mostOftenWeatherDescription
                            .GroupBy(i => IconConverter.CutDNSymbol(i.Icon, dayIconKey, nigthIconKey))
                            .OrderByDescending(i => i.Count())
                            .First();

                        //generate full icon url with daytime symbol as most common icon 
                        string avgIconUrl = IconConverter.Generate(
                            _wfProviderSettings.IconStorageAddress,
                            mostOftenIcon.Key,
                            dayIconKey
                        );
                        
                        var dailyForecast = new DailyForecastModel
                        {
                            Date = currentDay.ToWeatherDateFormat(item.Timezone),
                            DetailedDailyForecasts = detailedDailyForecasts,
                            AvgWeatherConditions = new WeatherConditionsModel
                            {
                                Humidity = (int)CalculateAverage(detailedDailyForecasts, i => i.WeatherConditions.Humidity, 0),
                                Pressure = (int)CalculateAverage(detailedDailyForecasts, i => i.WeatherConditions.Pressure, 0),
                                Temperature = (int)CalculateAverage(detailedDailyForecasts, i => i.WeatherConditions.Temperature, 0),
                                WindSpeed = CalculateAverage(detailedDailyForecasts, i => i.WeatherConditions.WindSpeed, 1),
                                IconUrl = avgIconUrl,
                                WeatherDescription = mostOftenWeatherDescription.Key, //set most common weather state as average weather
                                TemperatureFormat = temperatureFormat
                            }
                        };

                        //save day's forecast
                        forecast.DailyForecasts.Add(dailyForecast);

                        //clear objects for next day forecast
                        detailedDailyForecasts = new List<DetailedDailyForecastModel>();
                        weatherDetailsForAvg = new List<WeatherDetails>();

                        //move to next day
                        currentDay = nextDay;
                    }
                }

                return forecast;
            }
            catch (Exception ex)
            {
                string errorMessage = "Something went wrond during convertion from OpenWeatherForecast to ForecastModel.";
                _logger.LogError(errorMessage, ex);
                throw new Exception(errorMessage, ex);
            }
        }

        #region private methods
        private double CalculateAverage(
            IEnumerable<DetailedDailyForecastModel> forecasts,
            Func<DetailedDailyForecastModel, double> func,
            int precision
         )
        {
            return forecasts.Count() == 0
                ? 0
                : Math.Round(forecasts.Average(func), precision);
        }
        #endregion
    }
}
