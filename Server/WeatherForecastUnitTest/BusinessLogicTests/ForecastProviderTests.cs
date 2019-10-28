using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Net;
using WeatherForecast.BusinessLogic;
using WeatherForecast.Entities.Models;
using WeatherForecast.Entities.OpenWeatherApiEntities.OpenWeatherJsonEntities;
using WeatherForecast.Exceptions;
using WeatherForecast.Interfaces;
using WeatherForecast.JsonEntities.OpenWeatherForecast;

namespace WeatherForecast.UnitTests.BusinessLogicTests
{
    [TestFixture]
    class ForecastProviderTests
    {
        [Test]
        public void GetCurrentForecastByCityName_InvalidInput()
        {
            //Arrange
            var logger = new Mock<ILogger<IForecastProvider>>();
            var weatherApiRetriever = new Mock<IApiDataRetriever<OpenWeatherForecast>>();
            var uriProvider = new Mock<IExternalApiUriProvider>();
            var weatherResponseConverter = new Mock<IExternalApiResponseConverter<OpenWeatherForecast, ForecastModel>>();

            var weatherProvider = new ForecastProvider(
                logger.Object, 
                weatherApiRetriever.Object, 
                uriProvider.Object, 
                weatherResponseConverter.Object);

            //Act
            //Assert
            Assert.ThrowsAsync<HttpResponseWithStatusCodeException>(
                () => weatherProvider.GetForecastByCityName("", ""), "Invalid input parameters.");
        }

        [Test]
        public void GetForecastByCityName_GetData_Returns_EmptyOpenWeatherForecast()
        {
            //Arrange
            var logger = new Mock<ILogger<IForecastProvider>>();
            OpenWeatherForecast openWeatherForecast = new OpenWeatherForecast();

            var uriProvider = new Mock<IExternalApiUriProvider>();
            Uri uri = new Uri("http://someuri");
            uriProvider.Setup(m => m.GetForecastByCityUri(It.IsAny<string>(), It.IsAny<string>())).Returns(uri);

            var weatherApiRetriever = new Mock<IApiDataRetriever<OpenWeatherForecast>>();
            weatherApiRetriever.Setup(m => m.GetData(It.IsAny<Uri>(), It.IsAny<string>()))
                .ReturnsAsync(openWeatherForecast);

            var weatherResponseConverter = new Mock<IExternalApiResponseConverter<OpenWeatherForecast, ForecastModel>>();
            weatherResponseConverter
                .Setup(m => m.Convert(It.IsAny<OpenWeatherForecast>()))
                .Throws(new ArgumentNullException("Invalid input parameter."));

            var forecastProvider = new ForecastProvider(
                logger.Object, 
                weatherApiRetriever.Object, 
                uriProvider.Object, 
                weatherResponseConverter.Object);

            //Act
            //Assert
            Assert.ThrowsAsync<HttpResponseWithStatusCodeException>(
                () => forecastProvider.GetForecastByCityName("berlin", "de"), "Invalid input parameters.");
        }

        [Test]
        public void GetForecastByCityName_GetData_Returns_ApiError()
        {
            //Arrange
            var logger = new Mock<ILogger<IForecastProvider>>();

            var uriProvider = new Mock<IExternalApiUriProvider>();
            Uri uri = new Uri("http://someuri");
            uriProvider.Setup(m => m.GetForecastByCityUri(It.IsAny<string>(), It.IsAny<string>())).Returns(uri);

            var weatherApiRetriever = new Mock<IApiDataRetriever<OpenWeatherForecast>>();
            weatherApiRetriever.Setup(m => m.GetData(It.IsAny<Uri>(), It.IsAny<string>()))
                .Throws(new HttpResponseWithStatusCodeException(HttpStatusCode.NotFound, "city not found"));

            var weatherResponseConverter = new Mock<IExternalApiResponseConverter<OpenWeatherForecast, ForecastModel>>();

            var forecastProvider = new ForecastProvider(
                logger.Object, 
                weatherApiRetriever.Object, 
                uriProvider.Object, 
                weatherResponseConverter.Object);

            //Act
            //Assert
            Assert.ThrowsAsync<HttpResponseWithStatusCodeException>(
                () => forecastProvider.GetForecastByCityName("berlik", "de"), "Invalid input parameters.");
        }

        [Test]
        public void GetForecastByZipcode_InvalidInput()
        {
            //Arrange
            var logger = new Mock<ILogger<IForecastProvider>>();
            var weatherApiRetriever = new Mock<IApiDataRetriever<OpenWeatherForecast>>();
            var uriProvider = new Mock<IExternalApiUriProvider>();
            var weatherResponseConverter = new Mock<IExternalApiResponseConverter<OpenWeatherForecast, ForecastModel>>();

            var forecastProvider = new ForecastProvider(
                logger.Object, 
                weatherApiRetriever.Object, 
                uriProvider.Object, 
                weatherResponseConverter.Object);

            //Act
            //Assert
            Assert.ThrowsAsync<HttpResponseWithStatusCodeException>(
                () => forecastProvider.GetForecastByZipCode("", ""), "Invalid input parameters.");
        }

        //TODO finish
        [Test]
        public void GetForecastByZipcode_InvalidRequestUri()
        {
            //Arrange
            var logger = new Mock<ILogger<IForecastProvider>>();
            OpenWeatherForecast openWeatherForecast = new OpenWeatherForecast();

            var weatherApiRetriever = new Mock<IApiDataRetriever<OpenWeatherForecast>>();
            weatherApiRetriever.Setup(m => m.GetData(It.IsAny<Uri>(), It.IsAny<string>()))
                .ReturnsAsync(openWeatherForecast);

            var uriProvider = new Mock<IExternalApiUriProvider>();
            Uri invalidUri = new Uri("http://invaliduri");
            uriProvider.Setup(m => m.GetForecastByZipCodeUri(It.IsAny<string>(), It.IsAny<string>())).Returns(invalidUri);

            var weatherResponseConverter = new Mock<IExternalApiResponseConverter<OpenWeatherForecast, ForecastModel>>();

            var forecastProvider = new ForecastProvider(
                logger.Object, 
                weatherApiRetriever.Object, 
                uriProvider.Object, 
                weatherResponseConverter.Object);

            //Act
            var result = forecastProvider.GetForecastByZipCode("10247", "de"); //10247 - Berlin Friedrichshain

            //Assert
            Assert.ThrowsAsync<HttpResponseWithStatusCodeException>(
                () => forecastProvider.GetForecastByZipCode("berlin", "de"), "Invalid input parameters.");
        }

        [Test]
        public void GetForecastByZipCode_GetData_Returns_EmptyOpenWeather()
        {
            //Arrange
            var logger = new Mock<ILogger<IForecastProvider>>();
            OpenWeatherForecast openWeatherForecast = new OpenWeatherForecast();

            var uriProvider = new Mock<IExternalApiUriProvider>();
            Uri uri = new Uri("http://someuri");
            uriProvider.Setup(m => m.GetForecastByZipCodeUri(It.IsAny<string>(), It.IsAny<string>())).Returns(uri);

            var weatherApiRetriever = new Mock<IApiDataRetriever<OpenWeatherForecast>>();
            weatherApiRetriever.Setup(m => m.GetData(It.IsAny<Uri>(), It.IsAny<string>()))
                .ReturnsAsync(openWeatherForecast);

            var weatherResponseConverter = new Mock<IExternalApiResponseConverter<OpenWeatherForecast, ForecastModel>>();
            weatherResponseConverter.Setup(m => m.Convert(It.IsAny<OpenWeatherForecast>())).Throws(new Exception("Invalid input parameter."));

            var forecastProvider = new ForecastProvider(
                logger.Object, 
                weatherApiRetriever.Object, 
                uriProvider.Object, 
                weatherResponseConverter.Object);

            //Act
            //Assert
            Assert.ThrowsAsync<HttpResponseWithStatusCodeException>(
                () => forecastProvider.GetForecastByZipCode("10247", "de"), "Invalid input parameters.");
        }

        [Test]
        public void GetForecastByZipcode_GetData_Returns_ApiError()
        {
            //Arrange
            var logger = new Mock<ILogger<IForecastProvider>>();

            var uriProvider = new Mock<IExternalApiUriProvider>();
            Uri uri = new Uri("http://someuri");
            uriProvider.Setup(m => m.GetForecastByZipCodeUri(It.IsAny<string>(), It.IsAny<string>())).Returns(uri);

            var weatherApiRetriever = new Mock<IApiDataRetriever<OpenWeatherForecast>>();
            weatherApiRetriever.Setup(m => m.GetData(It.IsAny<Uri>(), It.IsAny<string>()))
                .Throws(new HttpResponseWithStatusCodeException(HttpStatusCode.NotFound, "city not found"));

            var weatherResponseConverter = new Mock<IExternalApiResponseConverter<OpenWeatherForecast, ForecastModel>>();

            var forecastProvider = new ForecastProvider(
                logger.Object, 
                weatherApiRetriever.Object, 
                uriProvider.Object, 
                weatherResponseConverter.Object);

            //Act
            //Assert
            Assert.ThrowsAsync<HttpResponseWithStatusCodeException>(
                () => forecastProvider.GetForecastByZipCode("70247", "de"), "Invalid input parameters.");
        }
    }
}
