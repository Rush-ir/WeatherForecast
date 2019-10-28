using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using WeatherForecast.ApiResponseConverters;
using WeatherForecast.Entities.ConfigurationSections;
using WeatherForecast.Entities.Models;
using WeatherForecast.Entities.OpenWeatherApiEntities;
using WeatherForecast.Entities.OpenWeatherApiEntities.OpenWeatherJsonEntities;
using WeatherForecast.Exceptions;

namespace WeatherForecast.UnitTests.ApiResponseConvertersTests
{
    [TestFixture]
    public class WeatherResponseConverterTests
    {
        [Test]
        public void WeatherResponseConverter_ConvertEmptyObject()
        {
            //Arrange
            var logger = new Mock<ILogger<WeatherResponseConverter>>();
            var weatherForecastProviderSettingsAccessor = new Mock<IOptions<WeatherForecastProviderSettings>>();

            WeatherResponseConverter converter = new WeatherResponseConverter(logger.Object, weatherForecastProviderSettingsAccessor.Object);
            OpenWeather openWeather = new OpenWeather();

            //Act
            //Assert
            Assert.Throws<Exception>(() => converter.Convert(openWeather), "Invalid input parameters.");
        }

        [Test]
        public void WeatherResponseConverter_InvalidSettings_WithoutIconStorageAddress()
        {
            //Arrange
            var logger = new Mock<ILogger<WeatherResponseConverter>>();
            var weatherForecastProviderSettingsAccessor = new Mock<IOptions<WeatherForecastProviderSettings>>();
            WeatherForecastProviderSettings settings = new WeatherForecastProviderSettings
            {
                UnitsFormat = UnitsFormats.Metric
            };
            weatherForecastProviderSettingsAccessor.SetupGet(m => m.Value).Returns(settings);

            WeatherResponseConverter converter = new WeatherResponseConverter(logger.Object, weatherForecastProviderSettingsAccessor.Object);
            OpenWeather openWeather = new OpenWeather
            {
                Weather = new List<WeatherDetails> { new WeatherDetails { Description = "rain", Icon = "10n" } },
                Wind = new Wind { Speed = 1.66999f },
                Main = new Main { Humidity = 1, Pressure = 2, Temp = 3.3f },
                Timezone = 3600, //utc+1
                Dt = 1571708528 //utc Monday, 22 October 2019, 1:42:08
            };

            CurrentWeatherModel expectedModel = new CurrentWeatherModel
            {
                Date = "02:42 22 October",
                WeatherConditions = new WeatherConditionsModel
                {
                    Humidity = 1,
                    IconUrl = "http://openweathermap.org/img/wn/10n@2x.png",
                    Pressure = 2,
                    Temperature = 3,
                    TemperatureFormat = "C",
                    WeatherDescription = "rain",
                    WindSpeed = 1.7d
                }
            };

            //Act
            //Assert
            Assert.Throws<Exception>(() => converter.Convert(openWeather), "Something went wrond during convertion from OpenWeather to CurrentWeatherModel.");
        }

        [Test]
        public void WeatherResponseConverter_CorrectInputParameter()
        {
            //Arrange
            var logger = new Mock<ILogger<WeatherResponseConverter>>();
            var weatherForecastProviderSettingsAccessor = new Mock<IOptions<WeatherForecastProviderSettings>>();
            WeatherForecastProviderSettings settings = new WeatherForecastProviderSettings 
            { 
                IconStorageAddress = "http://openweathermap.org/img/wn/iconcode@2x.png",
                UnitsFormat = UnitsFormats.Metric
            };
            weatherForecastProviderSettingsAccessor.SetupGet(m => m.Value).Returns(settings);

            WeatherResponseConverter converter = new WeatherResponseConverter(logger.Object, weatherForecastProviderSettingsAccessor.Object);
            OpenWeather openWeather = new OpenWeather 
            {
                Weather = new List<WeatherDetails> { new WeatherDetails { Description = "rain", Icon = "10n" } },
                Wind =  new Wind { Speed = 1.66999f },
                Main = new Main { Humidity = 1, Pressure = 2, Temp = 3.3f},
                Timezone = 3600, //utc+1
                Dt = 1571708528 //utc Monday, 22 October 2019, 1:42:08
            };

            CurrentWeatherModel expectedModel = new CurrentWeatherModel 
            { 
                Date = "02:42 22 October",
                WeatherConditions = new WeatherConditionsModel
                {
                    Humidity = 1,
                    IconUrl = "http://openweathermap.org/img/wn/10n@2x.png",
                    Pressure = 2,
                    Temperature = 3,
                    TemperatureFormat = "C",
                    WeatherDescription = "rain",
                    WindSpeed = 1.7d
                }
            };

            //Act
            var resultModel = converter.Convert(openWeather);

            //Assert
            Assert.AreEqual(expectedModel, resultModel);
        }
    }
}
