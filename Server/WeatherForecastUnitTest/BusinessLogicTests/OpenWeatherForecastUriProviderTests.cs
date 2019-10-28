using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using WeatherForecast.BusinessLogic;
using WeatherForecast.Entities.ConfigurationSections;
using WeatherForecast.Entities.OpenWeatherApiEntities;
using WeatherForecast.Exceptions;
using WeatherForecast.Interfaces;

namespace WeatherForecast.UnitTests.BusinessLogicTests
{
    [TestFixture]
    public class OpenWeatherForecastUriProviderTests
    {
        [Test]
        public void GetCurrentWeatherByCityUri_InvalidInput()
        {
            //Arrange
            var weatherForecastProviderSettingsAccessor = new Mock<IOptions<WeatherForecastProviderSettings>>();
            var logger = new Mock<ILogger<OpenWeatherForecastUriProvider>>();
            var provider = new OpenWeatherForecastUriProvider(logger.Object, weatherForecastProviderSettingsAccessor.Object);

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => provider.GetCurrentWeatherByCityUri("", ""), "Invalid input parameters.");
        }

        [Test]
        public void GetCurrentWeatherByCityUri_ValidInput()
        {
            //Arrange
            var weatherForecastProviderSettingsAccessor = new Mock<IOptions<WeatherForecastProviderSettings>>();
            weatherForecastProviderSettingsAccessor.SetupGet(m => m.Value).Returns(new WeatherForecastProviderSettings
            {
                SearchByCityParameter = "q",
                UnitsFormat = UnitsFormats.Metric,
                ApiKey = "somekey",
                BaseAddress = "https://api.openweathermap.org/data/2.5/"
            });
            var logger = new Mock<ILogger<OpenWeatherForecastUriProvider>>();
            var provider = new OpenWeatherForecastUriProvider(logger.Object, weatherForecastProviderSettingsAccessor.Object);

            Uri expectedResult = new Uri("https://api.openweathermap.org/data/2.5/weather?q=berlin,de&units=Metric&appid=somekey");

            //Act
            Uri result = provider.GetCurrentWeatherByCityUri("berlin", "de");

            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void GetCurrentWeatherByZipCodeUri_InvalidInput()
        {
            //Arrange
            var weatherForecastProviderSettingsAccessor = new Mock<IOptions<WeatherForecastProviderSettings>>();
            var logger = new Mock<ILogger<OpenWeatherForecastUriProvider>>();
            var provider = new OpenWeatherForecastUriProvider(logger.Object, weatherForecastProviderSettingsAccessor.Object);

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => provider.GetCurrentWeatherByZipCodeUri("", ""), "Invalid input parameters.");
        }

        [Test]
        public void GetCurrentWeatherByZipCode_ValidInput()
        {
            //Arrange
            var weatherForecastProviderSettingsAccessor = new Mock<IOptions<WeatherForecastProviderSettings>>();
            weatherForecastProviderSettingsAccessor.SetupGet(m => m.Value).Returns(new WeatherForecastProviderSettings
            {
                SearchByZipCodeParameter = "zip",
                UnitsFormat = UnitsFormats.Metric,
                ApiKey = "somekey",
                BaseAddress = "https://api.openweathermap.org/data/2.5/"
            });
            var logger = new Mock<ILogger<OpenWeatherForecastUriProvider>>();
            var provider = new OpenWeatherForecastUriProvider(logger.Object, weatherForecastProviderSettingsAccessor.Object);

            Uri expectedResult = new Uri("https://api.openweathermap.org/data/2.5/weather?zip=20095,de&units=Metric&appid=somekey");

            //Act
            var result = provider.GetCurrentWeatherByZipCodeUri("20095", "de");

            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void GetForecastByCityUri_InvalidInput()
        {
            //Arrange
            var weatherForecastProviderSettingsAccessor = new Mock<IOptions<WeatherForecastProviderSettings>>();
            var logger = new Mock<ILogger<OpenWeatherForecastUriProvider>>();
            var provider = new OpenWeatherForecastUriProvider(logger.Object, weatherForecastProviderSettingsAccessor.Object);

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => provider.GetForecastByCityUri("", ""), "Invalid input parameters.");
        }

        [Test]
        public void GetForecastByZipCode_ValidInput()
        {
            //Arrange
            var weatherForecastProviderSettingsAccessor = new Mock<IOptions<WeatherForecastProviderSettings>>();
            weatherForecastProviderSettingsAccessor.SetupGet(m => m.Value).Returns(new WeatherForecastProviderSettings
            {
                SearchByZipCodeParameter = "zip",
                UnitsFormat = UnitsFormats.Metric,
                ApiKey = "somekey",
                BaseAddress = "https://api.openweathermap.org/data/2.5/"
            });
            var logger = new Mock<ILogger<OpenWeatherForecastUriProvider>>();
            var provider = new OpenWeatherForecastUriProvider(logger.Object, weatherForecastProviderSettingsAccessor.Object);

            Uri expectedResult = new Uri("https://api.openweathermap.org/data/2.5/forecast?zip=20095,de&units=Metric&appid=somekey");

            //Act
            var result = provider.GetForecastByZipCodeUri("20095", "de");

            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void GetForecastByZipCodeUri_InvalidInput()
        {
            //Arrange
            var weatherForecastProviderSettingsAccessor = new Mock<IOptions<WeatherForecastProviderSettings>>();
            var logger = new Mock<ILogger<OpenWeatherForecastUriProvider>>();
            var provider = new OpenWeatherForecastUriProvider(logger.Object, weatherForecastProviderSettingsAccessor.Object);

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => provider.GetForecastByZipCodeUri("", ""), "Invalid input parameters.");
        }

        [Test]
        public void GetForecastByCityName_ValidInput()
        {
            //Arrange
            var weatherForecastProviderSettingsAccessor = new Mock<IOptions<WeatherForecastProviderSettings>>();
            weatherForecastProviderSettingsAccessor.SetupGet(m => m.Value).Returns(new WeatherForecastProviderSettings
            {
                SearchByCityParameter = "q",
                UnitsFormat = UnitsFormats.Metric,
                ApiKey = "somekey",
                BaseAddress = "https://api.openweathermap.org/data/2.5/"
            });
            var logger = new Mock<ILogger<OpenWeatherForecastUriProvider>>();
            var provider = new OpenWeatherForecastUriProvider(logger.Object, weatherForecastProviderSettingsAccessor.Object);

            Uri expectedResult = new Uri("https://api.openweathermap.org/data/2.5/forecast?q=berlin,de&units=Metric&appid=somekey");

            //Act
            var result = provider.GetForecastByCityUri("berlin", "de");

            //Assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}
