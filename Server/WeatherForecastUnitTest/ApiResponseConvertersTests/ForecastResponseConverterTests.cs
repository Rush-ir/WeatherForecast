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
using WeatherForecast.JsonEntities.OpenWeatherForecast;

namespace WeatherForecast.UnitTests.ApiResponseConvertersTests
{
    [TestFixture]
    public class ForecastResponseConverterTests
    {
        [Test]
        public void ForecastResponseConverter_ConvertEmptyObject()
        {
            //Arrange
            var logger = new Mock<ILogger<ForecastResponseConverter>>();
            var weatherForecastProviderSettingsAccessor = new Mock<IOptions<WeatherForecastProviderSettings>>();

            ForecastResponseConverter converter = new ForecastResponseConverter(logger.Object, weatherForecastProviderSettingsAccessor.Object);
            OpenWeatherForecast forecast = new OpenWeatherForecast();

            //Act
            //Assert
            Assert.Throws<Exception>(() => converter.Convert(forecast), "Invalid input parameters.");
        }

        [Test]
        public void ForecastResponseConverter_ConvertObject_WithNoForecasts()
        {
            //Arrange
            var logger = new Mock<ILogger<ForecastResponseConverter>>();
            var weatherForecastProviderSettingsAccessor = new Mock<IOptions<WeatherForecastProviderSettings>>();

            ForecastResponseConverter converter = new ForecastResponseConverter(logger.Object, weatherForecastProviderSettingsAccessor.Object);
            OpenWeatherForecast forecast = new OpenWeatherForecast 
            {
                City = new City { Name = "berlin" },
                List = new List<OpenWeather>()
            };


            //Act
            //Assert
            Assert.Throws<Exception>(() => converter.Convert(forecast), "External api forecast is empty.");
        }

        [Test]
        public void ForecastResponseConverter_ConvertObject_WithoutSettings()
        {
            //Arrange
            var logger = new Mock<ILogger<ForecastResponseConverter>>();
            var weatherForecastProviderSettingsAccessor = new Mock<IOptions<WeatherForecastProviderSettings>>();

            ForecastResponseConverter converter = new ForecastResponseConverter(logger.Object, weatherForecastProviderSettingsAccessor.Object);
            OpenWeatherForecast forecast = new OpenWeatherForecast
            {
                City = new City { Name = "berlin" },
                List = new List<OpenWeather>
                {
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails> { new WeatherDetails { Description = "rain", Icon = "10n" } },
                        Wind = new Wind { Speed = 1.66999f },
                        Main = new Main { Humidity = 1, Pressure = 2, Temp = 3.3f },
                        Timezone = 3600, //utc+1
                        Dt = 1571708528 //utc Monday, 22 October 2019, 1:42:08
                    }
                }
            };


            //Act
            //Assert
            Assert.Throws<Exception>(() => converter.Convert(forecast), "Something went wrond during convertion from OpenWeatherForecast to ForecastModel.");
        }

        [Test]
        public void ForecastResponseConverter_Convert_FullObject()
        {
            //Arrange
            var logger = new Mock<ILogger<ForecastResponseConverter>>();
            var weatherForecastProviderSettingsAccessor = new Mock<IOptions<WeatherForecastProviderSettings>>();
            WeatherForecastProviderSettings settings = new WeatherForecastProviderSettings
            {
                UnitsFormat = UnitsFormats.Metric,
                NigthIconKey = "n",
                DayIconKey = "d",
                IconStorageAddress = "http://openweathermap.org/img/wn/iconcode@2x.png"
            };
            weatherForecastProviderSettingsAccessor.SetupGet(m => m.Value).Returns(settings);

            ForecastResponseConverter converter = new ForecastResponseConverter(logger.Object, weatherForecastProviderSettingsAccessor.Object);
            OpenWeatherForecast forecast = new OpenWeatherForecast
            {
                City = new City { Name = "Cologne" },
                List = new List<OpenWeather>
                {
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails> 
                        { 
                            new WeatherDetails { Description = "clear sky", Icon = "01d", Main = "clear" } 
                        },
                        Wind = new Wind { Speed = 0.87f },
                        Main = new Main { Humidity = 69, Pressure = 1024, Temp = 6.38f },
                        Timezone = 3600, //utc+1
                        Dt = 1571734800 //utc Tuesday, 22 October 2019, 09:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails> 
                        { 
                            new WeatherDetails { Description = "clear sky", Icon = "01d", Main = "clear" } 
                        },
                        Wind = new Wind { Speed = 0.96f },
                        Main = new Main { Humidity = 55, Pressure = 1024, Temp = 10.3f },
                        Timezone = 3600, //utc+1
                        Dt = 1571745600 //utc Tuesday, 22 October 2019, 12:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails> 
                        { 
                            new WeatherDetails { Description = "broken clouds", Icon = "04d", Main = "clouds" } 
                        },
                        Wind = new Wind { Speed = 1.39f },
                        Main = new Main { Humidity = 57, Pressure = 1022, Temp = 10.49f },
                        Timezone = 3600, //utc+1
                        Dt = 1571756400 //utc Tuesday, 22 October 2019, 15:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails> 
                        { 
                            new WeatherDetails { Description = "broken clouds", Icon = "04n", Main = "clouds" } 
                        },
                        Wind = new Wind { Speed = 1.26f },
                        Main = new Main { Humidity = 65, Pressure = 1023, Temp = 9.17f },
                        Timezone = 3600, //utc+1
                        Dt = 1571767200 //utc Tuesday, 22 October 2019, 18:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails> 
                        { 
                            new WeatherDetails { Description = "overcast clouds", Icon = "04n", Main = "clouds" } 
                        },
                        Wind = new Wind { Speed = 1.94f },
                        Main = new Main { Humidity = 68, Pressure = 1023, Temp = 8.6f },
                        Timezone = 3600, //utc+1
                        Dt = 1571778000 //utc Tuesday, 22 October 2019, 21:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails> 
                        { 
                            new WeatherDetails { Description = "overcast clouds", Icon = "04n", Main = "clouds" } 
                        },
                        Wind = new Wind { Speed = 2.18f },
                        Main = new Main { Humidity = 71, Pressure = 1023, Temp = 7.96f },
                        Timezone = 3600, //utc+1
                        Dt = 1571788800 //utc Wednesday, 23 October 2019, 00:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails> 
                        { 
                            new WeatherDetails { Description = "overcast clouds", Icon = "04n", Main = "clouds" } 
                        },
                        Wind = new Wind { Speed = 1.9f },
                        Main = new Main { Humidity = 75, Pressure = 1023, Temp = 7.11f },
                        Timezone = 3600, //utc+1
                        Dt = 1571799600 //utc Wednesday, 23 October 2019, 03:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails> 
                        { 
                            new WeatherDetails { Description = "overcast clouds", Icon = "04n", Main = "clouds" } 
                        },
                        Wind = new Wind { Speed = 2.44f },
                        Main = new Main { Humidity = 77, Pressure = 1024, Temp = 6.52f },
                        Timezone = 3600, //utc+1
                        Dt = 1571810400 //utc Wednesday, 23 October 2019, 06:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails> 
                        { 
                            new WeatherDetails { Description = "scattered clouds", Icon = "03d", Main = "clouds" } 
                        },
                        Wind = new Wind { Speed = 2.47f },
                        Main = new Main { Humidity = 63, Pressure = 1026, Temp = 8.4f },
                        Timezone = 3600, //utc+1
                        Dt = 1571821200 //utc Wednesday, 23 October 2019, 09:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails> 
                        { 
                            new WeatherDetails { Description = "broken clouds", Icon = "04d", Main = "clouds" } 
                        },
                        Wind = new Wind { Speed = 2.77f },
                        Main = new Main { Humidity = 48, Pressure = 1026, Temp = 11.1f },
                        Timezone = 3600, //utc+1
                        Dt = 1571832000 //utc Wednesday, 23 October 2019, 12:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails> 
                        { 
                            new WeatherDetails { Description = "clear sky", Icon = "01d", Main = "clear" } 
                        },
                        Wind = new Wind { Speed = 3.32f },
                        Main = new Main { Humidity = 53, Pressure = 1026, Temp = 10.64f },
                        Timezone = 3600, //utc+1
                        Dt = 1571842800 //utc Wednesday, 23 October 2019, 15:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails> 
                        { 
                            new WeatherDetails { Description = "clear sky", Icon = "01n", Main = "clear" } 
                        },
                        Wind = new Wind { Speed = 3.91f },
                        Main = new Main { Humidity = 65, Pressure = 1028, Temp = 7.99f },
                        Timezone = 3600, //utc+1
                        Dt = 1571853600 //utc Wednesday, 23 October 2019, 18:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails> 
                        { 
                            new WeatherDetails { Description = "clear sky", Icon = "01n", Main = "clear" } 
                        },
                        Wind = new Wind { Speed = 3.65f },
                        Main = new Main { Humidity = 69, Pressure = 1030, Temp = 6.86f },
                        Timezone = 3600, //utc+1
                        Dt = 1571864400 //utc Wednesday, 23 October 2019, 21:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails> 
                        { 
                            new WeatherDetails { Description = "clear sky", Icon = "01n", Main = "clear" } 
                        },
                        Wind = new Wind { Speed = 2.71f },
                        Main = new Main { Humidity = 75, Pressure = 1030, Temp = 5.64f },
                        Timezone = 3600, //utc+1
                        Dt = 1571875200 //utc Thursday, 24 October 2019, 00:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails> 
                        { 
                            new WeatherDetails { Description = "clear sky", Icon = "01n", Main = "clear" } 
                        },
                        Wind = new Wind { Speed = 2.57f },
                        Main = new Main { Humidity = 79, Pressure = 1030, Temp = 4.91f },
                        Timezone = 3600, //utc+1
                        Dt = 1571886000 //utc Thursday, 24 October 2019, 03:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails> 
                        { 
                            new WeatherDetails { Description = "clear sky", Icon = "01n", Main = "clear" } 
                        },
                        Wind = new Wind { Speed = 2.32f },
                        Main = new Main { Humidity = 79, Pressure = 1030, Temp = 4.25f },
                        Timezone = 3600, //utc+1
                        Dt = 1571896800 //utc Thursday, 24 October 2019, 06:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails> 
                        { 
                            new WeatherDetails { Description = "clear sky", Icon = "01d", Main = "clear" } 
                        },
                        Wind = new Wind { Speed = 2.5f },
                        Main = new Main { Humidity = 66, Pressure = 1030, Temp = 6.8f },
                        Timezone = 3600, //utc+1
                        Dt = 1571907600 //utc Thursday, 24 October 2019, 09:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails> 
                        { 
                            new WeatherDetails { Description = "clear sky", Icon = "01d", Main = "clear" } 
                        },
                        Wind = new Wind { Speed = 3.06f },
                        Main = new Main { Humidity = 48, Pressure = 1029, Temp = 10.15f },
                        Timezone = 3600, //utc+1
                        Dt = 1571918400 //utc Thursday, 24 October 2019, 12:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails> 
                        { 
                            new WeatherDetails { Description = "clear sky", Icon = "01d", Main = "clear" } 
                        },
                        Wind = new Wind { Speed = 2.42f },
                        Main = new Main { Humidity = 49, Pressure = 1028, Temp = 10.03f },
                        Timezone = 3600, //utc+1
                        Dt = 1571929200 //utc Thursday, 24 October 2019, 15:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails>
                        {
                            new WeatherDetails { Description = "clear sky", Icon = "01n", Main = "clear" }
                        },
                        Wind = new Wind { Speed = 2.85f },
                        Main = new Main { Humidity = 61, Pressure = 1029, Temp = 6.94f },
                        Timezone = 3600, //utc+1
                        Dt = 1571940000 //utc Thursday, 24 October 2019, 18:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails>
                        {
                            new WeatherDetails { Description = "clear sky", Icon = "01n", Main = "clear" }
                        },
                        Wind = new Wind { Speed = 3.12f },
                        Main = new Main { Humidity = 64, Pressure = 1030, Temp = 4.84f },
                        Timezone = 3600, //utc+1
                        Dt = 1571950800 //utc Thursday, 24 October 2019, 21:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails>
                        {
                            new WeatherDetails { Description = "clear sky", Icon = "01n", Main = "clear" }
                        },
                        Wind = new Wind { Speed = 2.47f },
                        Main = new Main { Humidity = 67, Pressure = 1029, Temp = 3.88f },
                        Timezone = 3600, //utc+1
                        Dt = 1571961600 //utc Friday, 25 October 2019, 00:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails>
                        {
                            new WeatherDetails { Description = "clear sky", Icon = "01n", Main = "clear" }
                        },
                        Wind = new Wind { Speed = 2.74f },
                        Main = new Main { Humidity = 69, Pressure = 1028, Temp = 3.06f },
                        Timezone = 3600, //utc+1
                        Dt = 1571972400 //utc Friday, 25 October 2019, 03:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails>
                        {
                            new WeatherDetails { Description = "few clouds", Icon = "02n", Main = "clouds" }
                        },
                        Wind = new Wind { Speed = 2.62f },
                        Main = new Main { Humidity = 69, Pressure = 1027, Temp = 2.34f },
                        Timezone = 3600, //utc+1
                        Dt = 1571983200 //utc Friday, 25 October 2019, 06:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails>
                        {
                            new WeatherDetails { Description = "broken clouds", Icon = "04d", Main = "clouds" }
                        },
                        Wind = new Wind { Speed = 3.33f },
                        Main = new Main { Humidity = 56, Pressure = 1026, Temp = 5.02f },
                        Timezone = 3600, //utc+1
                        Dt = 1571994000 //utc Friday, 25 October 2019, 09:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails>
                        {
                            new WeatherDetails { Description = "broken clouds", Icon = "04d", Main = "clouds" }
                        },
                        Wind = new Wind { Speed = 3.04f },
                        Main = new Main { Humidity = 43, Pressure = 1025, Temp = 8.5f },
                        Timezone = 3600, //utc+1
                        Dt = 1572004800 //utc Friday, 25 October 2019, 12:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails>
                        {
                            new WeatherDetails { Description = "overcast clouds", Icon = "04d", Main = "clouds" }
                        },
                        Wind = new Wind { Speed = 2.51f },
                        Main = new Main { Humidity = 49, Pressure = 1023, Temp = 8.31f },
                        Timezone = 3600, //utc+1
                        Dt = 1572015600 //utc Friday, 25 October 2019, 15:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails>
                        {
                            new WeatherDetails { Description = "overcast clouds", Icon = "04n", Main = "clouds" }
                        },
                        Wind = new Wind { Speed = 3.27f },
                        Main = new Main { Humidity = 55, Pressure = 1022, Temp = 6.74f },
                        Timezone = 3600, //utc+1
                        Dt = 1572026400 //utc Friday, 25 October 2019, 18:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails>
                        {
                            new WeatherDetails { Description = "overcast clouds", Icon = "04n", Main = "clouds" }
                        },
                        Wind = new Wind { Speed = 2.68f },
                        Main = new Main { Humidity = 62, Pressure = 1021, Temp = 6.03f },
                        Timezone = 3600, //utc+1
                        Dt = 1572037200 //utc Friday, 25 October 2019, 21:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails>
                        {
                            new WeatherDetails { Description = "overcast clouds", Icon = "04n", Main = "clouds" }
                        },
                        Wind = new Wind { Speed = 3.09f },
                        Main = new Main { Humidity = 67, Pressure = 1019, Temp = 4.85f },
                        Timezone = 3600, //utc+1
                        Dt = 1572048000 //utc Saturday, 26 October 2019, 00:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails>
                        {
                            new WeatherDetails { Description = "overcast clouds", Icon = "04n", Main = "clouds" }
                        },
                        Wind = new Wind { Speed = 3.4f },
                        Main = new Main { Humidity = 74, Pressure = 1017, Temp = 4.36f },
                        Timezone = 3600, //utc+1
                        Dt = 1572058800 //utc Saturday, 26 October 2019, 03:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails>
                        {
                            new WeatherDetails { Description = "overcast clouds", Icon = "04n", Main = "clouds" }
                        },
                        Wind = new Wind { Speed = 4.16f },
                        Main = new Main { Humidity = 81, Pressure = 1013, Temp = 4.7f },
                        Timezone = 3600, //utc+1
                        Dt = 1572069600 //utc Saturday, 26 October 2019, 06:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails>
                        {
                            new WeatherDetails { Description = "ligth rain", Icon = "10d", Main = "rain" }
                        },
                        Wind = new Wind { Speed = 5.47f },
                        Main = new Main { Humidity = 77, Pressure = 1009, Temp = 7.71f },
                        Timezone = 3600, //utc+1
                        Dt = 1572080400 //utc Saturday, 26 October 2019, 09:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails>
                        {
                            new WeatherDetails { Description = "moderate rain", Icon = "10d", Main = "rain" }
                        },
                        Wind = new Wind { Speed = 5.39f },
                        Main = new Main { Humidity = 90, Pressure = 1005, Temp = 8.2f },
                        Timezone = 3600, //utc+1
                        Dt = 1572091200 //utc Saturday, 26 October 2019, 12:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails>
                        {
                            new WeatherDetails { Description = "moderate rain", Icon = "10d", Main = "rain" }
                        },
                        Wind = new Wind { Speed = 3.07f },
                        Main = new Main { Humidity = 93, Pressure = 1002, Temp = 10.35f },
                        Timezone = 3600, //utc+1
                        Dt = 1572102000 //utc Saturday, 26 October 2019, 15:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails>
                        {
                            new WeatherDetails { Description = "light rain", Icon = "10n", Main = "rain" }
                        },
                        Wind = new Wind { Speed = 2.94f },
                        Main = new Main { Humidity = 94, Pressure = 1001, Temp = 11.65f },
                        Timezone = 3600, //utc+1
                        Dt = 1572112800 //utc Saturday, 26 October 2019, 18:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails>
                        {
                            new WeatherDetails { Description = "light rain", Icon = "10n", Main = "rain" }
                        },
                        Wind = new Wind { Speed = 2.8f },
                        Main = new Main { Humidity = 92, Pressure = 1002, Temp = 10.69f },
                        Timezone = 3600, //utc+1
                        Dt = 1572123600 //utc Saturday, 26 October 2019, 21:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails>
                        {
                            new WeatherDetails { Description = "overcast clouds", Icon = "04n", Main = "clouds" }
                        },
                        Wind = new Wind { Speed = 2.74f },
                        Main = new Main { Humidity = 89, Pressure = 1001, Temp = 10.4f },
                        Timezone = 3600, //utc+1
                        Dt = 1572134400 //utc Sunday, 27 October 2019, 00:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails>
                        {
                            new WeatherDetails { Description = "broken clouds", Icon = "04n", Main = "clouds"}
                        },
                        Wind = new Wind { Speed = 2.24f },
                        Main = new Main { Humidity = 88, Pressure = 1000, Temp = 10.47f },
                        Timezone = 3600, //utc+1
                        Dt = 1572145200 //utc Sunday, 27 October 2019, 03:00:00
                    },
                    new OpenWeather
                    {
                        Weather = new List<WeatherDetails>
                        {
                            new WeatherDetails { Description = "overcast clouds", Icon = "04n", Main = "clouds" }
                        },
                        Wind = new Wind { Speed = 2.29f },
                        Main = new Main { Humidity = 89, Pressure = 999, Temp = 10f },
                        Timezone = 3600, //utc+1
                        Dt = 1572156000 //utc Sunday, 27 October 2019, 06:00:00
                    }
                }
            };

            ForecastModel expectedModel = new ForecastModel 
            { 
                City = "Cologne",
                DailyForecasts = new List<DailyForecastModel>
                {
                    new DailyForecastModel
                    {
                        Date = "22 October",
                        AvgWeatherConditions = new WeatherConditionsModel
                        {
                            Humidity = 63,
                            IconUrl = "http://openweathermap.org/img/wn/04d@2x.png",
                            Pressure = 1023,
                            Temperature = 9,
                            TemperatureFormat = "C",
                            WeatherDescription = "clouds",
                            WindSpeed = 1.3                            
                        },
                        DetailedDailyForecasts = new List<DetailedDailyForecastModel>
                        {
                            new DetailedDailyForecastModel
                            {
                                Time = "10:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 69,
                                    IconUrl = "http://openweathermap.org/img/wn/01d@2x.png",
                                    Pressure = 1024,
                                    Temperature = 6,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "clear sky",
                                    WindSpeed = 0.9
                                }
                            },                 
                            new DetailedDailyForecastModel
                            {
                                Time = "13:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 55,
                                    IconUrl = "http://openweathermap.org/img/wn/01d@2x.png",
                                    Pressure = 1024,
                                    Temperature = 10,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "clear sky",
                                    WindSpeed = 1
                                }
                            },
                            new DetailedDailyForecastModel
                            {
                                Time = "16:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 57,
                                    IconUrl = "http://openweathermap.org/img/wn/04d@2x.png",
                                    Pressure = 1022,
                                    Temperature = 10,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "broken clouds",
                                    WindSpeed = 1.4
                                }
                            },
                            new DetailedDailyForecastModel
                            {
                                Time = "19:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 65,
                                    IconUrl = "http://openweathermap.org/img/wn/04n@2x.png",
                                    Pressure = 1023,
                                    Temperature = 9,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "broken clouds",
                                    WindSpeed = 1.3
                                }
                            },
                            new DetailedDailyForecastModel
                            {
                                Time = "22:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 68,
                                    IconUrl = "http://openweathermap.org/img/wn/04n@2x.png",
                                    Pressure = 1023,
                                    Temperature = 9,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "overcast clouds",
                                    WindSpeed = 1.9
                                }
                            }
                        }
                    },
                    new DailyForecastModel
                    {
                        Date = "23 October",
                        AvgWeatherConditions = new WeatherConditionsModel
                        {
                            Humidity = 65,
                            IconUrl = "http://openweathermap.org/img/wn/04d@2x.png",
                            Pressure = 1026,
                            Temperature = 8,
                            TemperatureFormat = "C",
                            WeatherDescription = "clouds",
                            WindSpeed = 2.8
                        },
                        DetailedDailyForecasts = new List<DetailedDailyForecastModel>
                        {
                            new DetailedDailyForecastModel
                            {
                                Time = "01:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 71,
                                    IconUrl = "http://openweathermap.org/img/wn/04n@2x.png",
                                    Pressure = 1023,
                                    Temperature = 8,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "overcast clouds",
                                    WindSpeed = 2.2
                                }
                            },
                            new DetailedDailyForecastModel
                            {
                                Time = "04:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 75,
                                    IconUrl = "http://openweathermap.org/img/wn/04n@2x.png",
                                    Pressure = 1023,
                                    Temperature = 7,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "overcast clouds",
                                    WindSpeed = 1.9
                                }
                            },
                            new DetailedDailyForecastModel
                            {
                                Time = "07:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 77,
                                    IconUrl = "http://openweathermap.org/img/wn/04n@2x.png",
                                    Pressure = 1024,
                                    Temperature = 7,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "overcast clouds",
                                    WindSpeed = 2.4
                                }
                            },
                            new DetailedDailyForecastModel
                            {
                                Time = "10:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 63,
                                    IconUrl = "http://openweathermap.org/img/wn/03d@2x.png",
                                    Pressure = 1026,
                                    Temperature = 8,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "scattered clouds",
                                    WindSpeed = 2.5
                                }
                            },
                            new DetailedDailyForecastModel
                            {
                                Time = "13:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 48,
                                    IconUrl = "http://openweathermap.org/img/wn/04d@2x.png",
                                    Pressure = 1026,
                                    Temperature = 11,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "broken clouds",
                                    WindSpeed = 2.8
                                }
                            },
                            new DetailedDailyForecastModel
                            {
                                Time = "16:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 53,
                                    IconUrl = "http://openweathermap.org/img/wn/01d@2x.png",
                                    Pressure = 1026,
                                    Temperature = 11,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "clear sky",
                                    WindSpeed = 3.3
                                }
                            },
                            new DetailedDailyForecastModel
                            {
                                Time = "19:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 65,
                                    IconUrl = "http://openweathermap.org/img/wn/01n@2x.png",
                                    Pressure = 1028,
                                    Temperature = 8,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "clear sky",
                                    WindSpeed = 3.9
                                }
                            },
                            new DetailedDailyForecastModel
                            {
                                Time = "22:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 69,
                                    IconUrl = "http://openweathermap.org/img/wn/01n@2x.png",
                                    Pressure = 1030,
                                    Temperature = 7,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "clear sky",
                                    WindSpeed = 3.7
                                }
                            }
                        }
                    },
                    new DailyForecastModel
                    {
                        Date = "24 October",
                        AvgWeatherConditions = new WeatherConditionsModel
                        {
                            Humidity = 65,
                            IconUrl = "http://openweathermap.org/img/wn/01d@2x.png",
                            Pressure = 1030,
                            Temperature = 7,
                            TemperatureFormat = "C",
                            WeatherDescription = "clear",
                            WindSpeed = 2.7
                        },
                        DetailedDailyForecasts = new List<DetailedDailyForecastModel>
                        {
                            new DetailedDailyForecastModel
                            {
                                Time = "01:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 75,
                                    IconUrl = "http://openweathermap.org/img/wn/01n@2x.png",
                                    Pressure = 1030,
                                    Temperature = 6,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "clear sky",
                                    WindSpeed = 2.7
                                }
                            },
                            new DetailedDailyForecastModel
                            {
                                Time = "04:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 79,
                                    IconUrl = "http://openweathermap.org/img/wn/01n@2x.png",
                                    Pressure = 1030,
                                    Temperature = 5,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "clear sky",
                                    WindSpeed = 2.6
                                }
                            },
                            new DetailedDailyForecastModel
                            {
                                Time = "07:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 79,
                                    IconUrl = "http://openweathermap.org/img/wn/01n@2x.png",
                                    Pressure = 1030,
                                    Temperature = 4,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "clear sky",
                                    WindSpeed = 2.3
                                }
                            },
                            new DetailedDailyForecastModel
                            {
                                Time = "10:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 66,
                                    IconUrl = "http://openweathermap.org/img/wn/01d@2x.png",
                                    Pressure = 1030,
                                    Temperature = 7,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "clear sky",
                                    WindSpeed = 2.5
                                }
                            },
                            new DetailedDailyForecastModel
                            {
                                Time = "13:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 48,
                                    IconUrl = "http://openweathermap.org/img/wn/01d@2x.png",
                                    Pressure = 1029,
                                    Temperature = 10,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "clear sky",
                                    WindSpeed = 3.1
                                }
                            },
                            new DetailedDailyForecastModel
                            {
                                Time = "16:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 49,
                                    IconUrl = "http://openweathermap.org/img/wn/01d@2x.png",
                                    Pressure = 1028,
                                    Temperature = 10,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "clear sky",
                                    WindSpeed = 2.4
                                }
                            },
                            new DetailedDailyForecastModel
                            {
                                Time = "19:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 61,
                                    IconUrl = "http://openweathermap.org/img/wn/01n@2x.png",
                                    Pressure = 1029,
                                    Temperature = 7,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "clear sky",
                                    WindSpeed = 2.8
                                }
                            },
                            new DetailedDailyForecastModel
                            {
                                Time = "22:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 64,
                                    IconUrl = "http://openweathermap.org/img/wn/01n@2x.png",
                                    Pressure = 1030,
                                    Temperature = 5,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "clear sky",
                                    WindSpeed = 3.1
                                }
                            }
                        }
                    },
                    new DailyForecastModel
                    {
                        Date = "25 October",
                        AvgWeatherConditions = new WeatherConditionsModel
                        {
                            Humidity = 59,
                            IconUrl = "http://openweathermap.org/img/wn/04d@2x.png",
                            Pressure = 1025,
                            Temperature = 5,
                            TemperatureFormat = "C",
                            WeatherDescription = "clouds",
                            WindSpeed = 2.8
                        },
                        DetailedDailyForecasts = new List<DetailedDailyForecastModel>
                        {
                            new DetailedDailyForecastModel
                            {
                                Time = "01:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 67,
                                    IconUrl = "http://openweathermap.org/img/wn/01n@2x.png",
                                    Pressure = 1029,
                                    Temperature = 4,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "clear sky",
                                    WindSpeed = 2.5
                                }
                            },
                            new DetailedDailyForecastModel
                            {
                                Time = "04:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 69,
                                    IconUrl = "http://openweathermap.org/img/wn/01n@2x.png",
                                    Pressure = 1028,
                                    Temperature = 3,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "clear sky",
                                    WindSpeed = 2.7
                                }
                            },
                            new DetailedDailyForecastModel
                            {
                                Time = "07:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 69,
                                    IconUrl = "http://openweathermap.org/img/wn/02n@2x.png",
                                    Pressure = 1027,
                                    Temperature = 2,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "few clouds",
                                    WindSpeed = 2.6
                                }
                            },
                            new DetailedDailyForecastModel
                            {
                                Time = "10:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 56,
                                    IconUrl = "http://openweathermap.org/img/wn/04d@2x.png",
                                    Pressure = 1026,
                                    Temperature = 5,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "broken clouds",
                                    WindSpeed = 3.3
                                }
                            },
                            new DetailedDailyForecastModel
                            {
                                Time = "13:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 43,
                                    IconUrl = "http://openweathermap.org/img/wn/04d@2x.png",
                                    Pressure = 1025,
                                    Temperature = 8,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "broken clouds",
                                    WindSpeed = 3.0
                                }
                            },
                            new DetailedDailyForecastModel
                            {
                                Time = "16:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 49,
                                    IconUrl = "http://openweathermap.org/img/wn/04d@2x.png",
                                    Pressure = 1023,
                                    Temperature = 8,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "overcast clouds",
                                    WindSpeed = 2.5
                                }
                            },
                            new DetailedDailyForecastModel
                            {
                                Time = "19:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 55,
                                    IconUrl = "http://openweathermap.org/img/wn/04n@2x.png",
                                    Pressure = 1022,
                                    Temperature = 7,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "overcast clouds",
                                    WindSpeed = 3.3
                                }
                            },
                            new DetailedDailyForecastModel
                            {
                                Time = "22:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 62,
                                    IconUrl = "http://openweathermap.org/img/wn/04n@2x.png",
                                    Pressure = 1021,
                                    Temperature = 6,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "overcast clouds",
                                    WindSpeed = 2.7
                                }
                            }
                        }
                    },
                    new DailyForecastModel
                    {
                        Date = "26 October",
                        AvgWeatherConditions = new WeatherConditionsModel
                        {
                            Humidity = 84,
                            IconUrl = "http://openweathermap.org/img/wn/10d@2x.png",
                            Pressure = 1008,
                            Temperature = 8,
                            TemperatureFormat = "C",
                            WeatherDescription = "rain",
                            WindSpeed = 3.8
                        },
                        DetailedDailyForecasts = new List<DetailedDailyForecastModel>
                        {
                            new DetailedDailyForecastModel
                            {
                                Time = "01:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 67,
                                    IconUrl = "http://openweathermap.org/img/wn/04n@2x.png",
                                    Pressure = 1019,
                                    Temperature = 5,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "overcast clouds",
                                    WindSpeed = 3.1
                                }
                            },
                            new DetailedDailyForecastModel
                            {
                                Time = "04:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 74,
                                    IconUrl = "http://openweathermap.org/img/wn/04n@2x.png",
                                    Pressure = 1017,
                                    Temperature = 4,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "overcast clouds",
                                    WindSpeed = 3.4
                                }
                            },
                            new DetailedDailyForecastModel
                            {
                                Time = "07:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 81,
                                    IconUrl = "http://openweathermap.org/img/wn/04n@2x.png",
                                    Pressure = 1013,
                                    Temperature = 5,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "overcast clouds",
                                    WindSpeed = 4.2
                                }
                            },
                            new DetailedDailyForecastModel
                            {
                                Time = "10:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 77,
                                    IconUrl = "http://openweathermap.org/img/wn/10d@2x.png",
                                    Pressure = 1009,
                                    Temperature = 8,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "ligth rain",
                                    WindSpeed = 5.5
                                }
                            },
                            new DetailedDailyForecastModel
                            {
                                Time = "13:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 90,
                                    IconUrl = "http://openweathermap.org/img/wn/10d@2x.png",
                                    Pressure = 1005,
                                    Temperature = 8,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "moderate rain",
                                    WindSpeed = 5.4
                                }
                            },
                            new DetailedDailyForecastModel
                            {
                                Time = "16:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 93,
                                    IconUrl = "http://openweathermap.org/img/wn/10d@2x.png",
                                    Pressure = 1002,
                                    Temperature = 10,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "moderate rain",
                                    WindSpeed = 3.1
                                }
                            },
                            new DetailedDailyForecastModel
                            {
                                Time = "19:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 94,
                                    IconUrl = "http://openweathermap.org/img/wn/10n@2x.png",
                                    Pressure = 1001,
                                    Temperature = 12,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "light rain",
                                    WindSpeed = 2.9
                                }
                            },
                            new DetailedDailyForecastModel
                            {
                                Time = "22:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 92,
                                    IconUrl = "http://openweathermap.org/img/wn/10n@2x.png",
                                    Pressure = 1002,
                                    Temperature = 11,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "light rain",
                                    WindSpeed = 2.8
                                }
                            }
                        }
                    },
                    new DailyForecastModel
                    {
                        Date = "27 October",
                        AvgWeatherConditions = new WeatherConditionsModel
                        {
                            Humidity = 89,
                            IconUrl = "http://openweathermap.org/img/wn/04d@2x.png",
                            Pressure = 1000,
                            Temperature = 10,
                            TemperatureFormat = "C",
                            WeatherDescription = "clouds",
                            WindSpeed = 2.4
                        },
                        DetailedDailyForecasts = new List<DetailedDailyForecastModel>
                        {
                            new DetailedDailyForecastModel
                            {
                                Time = "01:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 89,
                                    IconUrl = "http://openweathermap.org/img/wn/04n@2x.png",
                                    Pressure = 1001,
                                    Temperature = 10,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "overcast clouds",
                                    WindSpeed = 2.7
                                }
                            },
                            new DetailedDailyForecastModel
                            {
                                Time = "04:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 88,
                                    IconUrl = "http://openweathermap.org/img/wn/04n@2x.png",
                                    Pressure = 1000,
                                    Temperature = 10,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "broken clouds",
                                    WindSpeed = 2.2
                                }
                            },
                            new DetailedDailyForecastModel
                            {
                                Time = "07:00",
                                WeatherConditions = new WeatherConditionsModel
                                {
                                    Humidity = 89,
                                    IconUrl = "http://openweathermap.org/img/wn/04n@2x.png",
                                    Pressure = 999,
                                    Temperature = 10,
                                    TemperatureFormat = "C",
                                    WeatherDescription = "overcast clouds",
                                    WindSpeed = 2.3
                                }
                            }
                        }
                    }
                }
            };

            //Act
            var result = converter.Convert(forecast);

            //Assert
            Assert.AreEqual(expectedModel, result);
        }
    }
}
