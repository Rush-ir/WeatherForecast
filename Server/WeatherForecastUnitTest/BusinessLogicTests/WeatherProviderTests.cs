using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using WeatherForecast.BusinessLogic;
using WeatherForecast.Entities.Models;
using WeatherForecast.Entities.OpenWeatherApiEntities.OpenWeatherJsonEntities;
using WeatherForecast.Exceptions;
using WeatherForecast.Interfaces;

namespace WeatherForecast.UnitTests.BusinessLogicTests
{
    [TestFixture]
    class WeatherProviderTests
    {
        [Test]
        public void GetCurrentWeatherByCityName_InvalidInput()
        {
            //Arrange
            var logger = new Mock<ILogger<IWeatherProvider>>();
            var weatherApiRetriever = new Mock<IApiDataRetriever<OpenWeather>>();
            var uriProvider = new Mock<IExternalApiUriProvider>();
            var weatherResponseConverter = new Mock<IExternalApiResponseConverter<OpenWeather, CurrentWeatherModel>>();

            var weatherProvider = new WeatherProvider(logger.Object, weatherApiRetriever.Object, uriProvider.Object, weatherResponseConverter.Object);

            //Act
            //Assert
            Assert.ThrowsAsync<HttpResponseWithStatusCodeException>(() => weatherProvider.GetCurrentWeatherByCityName("", ""), "Invalid input parameters.");
        }

        [Test]
        public void GetCurrentWeatherByCityName_GetData_Returns_EmptyOpenWeather()
        {
            //Arrange
            var logger = new Mock<ILogger<IWeatherProvider>>();
            OpenWeather openWeather = new OpenWeather();

            var uriProvider = new Mock<IExternalApiUriProvider>();
            Uri uri = new Uri("http://someuri");
            uriProvider.Setup(m => m.GetCurrentWeatherByCityUri(It.IsAny<string>(), It.IsAny<string>())).Returns(uri);

            var weatherApiRetriever = new Mock<IApiDataRetriever<OpenWeather>>();
            weatherApiRetriever.Setup(m => m.GetData(It.IsAny<Uri>(), It.IsAny<string>()))
                .ReturnsAsync(openWeather);

            var weatherResponseConverter = new Mock<IExternalApiResponseConverter<OpenWeather, CurrentWeatherModel>>();
            weatherResponseConverter.Setup(m => m.Convert(It.IsAny<OpenWeather>())).Throws(new Exception("Invalid input parameter."));

            var weatherProvider = new WeatherProvider(logger.Object, weatherApiRetriever.Object, uriProvider.Object, weatherResponseConverter.Object);

            //Act
            //Assert
            Assert.ThrowsAsync<HttpResponseWithStatusCodeException>(
                () => weatherProvider.GetCurrentWeatherByCityName("berlin", "de"), "Invalid input parameters.");
        }

        [Test]
        public void GetCurrentWeatherByCityName_GetData_Returns_ApiError()
        {
            //Arrange
            var logger = new Mock<ILogger<IWeatherProvider>>();

            var uriProvider = new Mock<IExternalApiUriProvider>();
            Uri uri = new Uri("http://someuri");
            uriProvider.Setup(m => m.GetCurrentWeatherByCityUri(It.IsAny<string>(), It.IsAny<string>())).Returns(uri);

            var weatherApiRetriever = new Mock<IApiDataRetriever<OpenWeather>>();
            weatherApiRetriever.Setup(m => m.GetData(It.IsAny<Uri>(), It.IsAny<string>()))
                .Throws(new HttpResponseWithStatusCodeException(HttpStatusCode.NotFound, "city not found"));

            var weatherResponseConverter = new Mock<IExternalApiResponseConverter<OpenWeather, CurrentWeatherModel>>();

            var weatherProvider = new WeatherProvider(logger.Object, weatherApiRetriever.Object, uriProvider.Object, weatherResponseConverter.Object);

            //Act
            //Assert
            Assert.ThrowsAsync<HttpResponseWithStatusCodeException>(
                () => weatherProvider.GetCurrentWeatherByCityName("berlik", "de"), "Invalid input parameters.");
        }

        [Test]
        public void GetCurrentWeatherByZipcode_InvalidInput()
        {
            //Arrange
            var logger = new Mock<ILogger<IWeatherProvider>>();
            var weatherApiRetriever = new Mock<IApiDataRetriever<OpenWeather>>();
            var uriProvider = new Mock<IExternalApiUriProvider>();
            var weatherResponseConverter = new Mock<IExternalApiResponseConverter<OpenWeather, CurrentWeatherModel>>();

            var weatherProvider = new WeatherProvider(logger.Object, weatherApiRetriever.Object, uriProvider.Object, weatherResponseConverter.Object);

            //Act
            //Assert
            Assert.ThrowsAsync<HttpResponseWithStatusCodeException>(() => weatherProvider.GetCurrentWeatherByZipCode("", ""), "Invalid input parameters.");
        }

        //TODO finish
        [Test]
        public void GetCurrentWeatherByZipcode_InvalidRequestUri()
        {
            //Arrange
            var logger = new Mock<ILogger<IWeatherProvider>>();
            OpenWeather openWeather = new OpenWeather();

            var weatherApiRetriever = new Mock<IApiDataRetriever<OpenWeather>>();
            weatherApiRetriever.Setup(m => m.GetData(It.IsAny<Uri>(), It.IsAny<string>()))
                .ReturnsAsync(openWeather);

            var uriProvider = new Mock<IExternalApiUriProvider>();
            Uri invalidUri = new Uri("http://invaliduri");
            uriProvider.Setup(m => m.GetCurrentWeatherByZipCodeUri(It.IsAny<string>(), It.IsAny<string>())).Returns(invalidUri);

            var weatherResponseConverter = new Mock<IExternalApiResponseConverter<OpenWeather, CurrentWeatherModel>>();

            var weatherProvider = new WeatherProvider(logger.Object, weatherApiRetriever.Object, uriProvider.Object, weatherResponseConverter.Object);

            //Act
            var result = weatherProvider.GetCurrentWeatherByZipCode("10247", "de"); //10247 - Berlin Friedrichshain

            //Assert
            Assert.ThrowsAsync<HttpResponseWithStatusCodeException>(
                () => weatherProvider.GetCurrentWeatherByZipCode("berlin", "de"), "Invalid input parameters.");
        }

        [Test]
        public void GetCurrentWeatherByZipCode_GetData_Returns_EmptyOpenWeather()
        {
            //Arrange
            var logger = new Mock<ILogger<IWeatherProvider>>();
            OpenWeather openWeather = new OpenWeather();

            var uriProvider = new Mock<IExternalApiUriProvider>();
            Uri uri = new Uri("http://someuri");
            uriProvider.Setup(m => m.GetCurrentWeatherByZipCodeUri(It.IsAny<string>(), It.IsAny<string>())).Returns(uri);

            var weatherApiRetriever = new Mock<IApiDataRetriever<OpenWeather>>();
            weatherApiRetriever.Setup(m => m.GetData(It.IsAny<Uri>(), It.IsAny<string>()))
                .ReturnsAsync(openWeather);

            var weatherResponseConverter = new Mock<IExternalApiResponseConverter<OpenWeather, CurrentWeatherModel>>();
            weatherResponseConverter.Setup(m => m.Convert(It.IsAny<OpenWeather>())).Throws(new Exception("Invalid input parameter."));

            var weatherProvider = new WeatherProvider(logger.Object, weatherApiRetriever.Object, uriProvider.Object, weatherResponseConverter.Object);

            //Act
            //Assert
            Assert.ThrowsAsync<HttpResponseWithStatusCodeException>(
                () => weatherProvider.GetCurrentWeatherByZipCode("10247", "de"), "Invalid input parameters.");
        }

        [Test]
        public void GetCurrentWeatherByZipcode_GetData_Returns_ApiError()
        {
            //Arrange
            var logger = new Mock<ILogger<IWeatherProvider>>();

            var uriProvider = new Mock<IExternalApiUriProvider>();
            Uri uri = new Uri("http://someuri");
            uriProvider.Setup(m => m.GetCurrentWeatherByZipCodeUri(It.IsAny<string>(), It.IsAny<string>())).Returns(uri);

            var weatherApiRetriever = new Mock<IApiDataRetriever<OpenWeather>>();
            weatherApiRetriever.Setup(m => m.GetData(It.IsAny<Uri>(), It.IsAny<string>()))
                .Throws(new HttpResponseWithStatusCodeException(HttpStatusCode.NotFound, "city not found"));

            var weatherResponseConverter = new Mock<IExternalApiResponseConverter<OpenWeather, CurrentWeatherModel>>();

            var weatherProvider = new WeatherProvider(logger.Object, weatherApiRetriever.Object, uriProvider.Object, weatherResponseConverter.Object);

            //Act
            //Assert
            Assert.ThrowsAsync<HttpResponseWithStatusCodeException>(
                () => weatherProvider.GetCurrentWeatherByZipCode("70247", "de"), "Invalid input parameters.");
        }
    }
}
