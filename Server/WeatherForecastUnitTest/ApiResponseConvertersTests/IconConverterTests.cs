using NUnit.Framework;
using System;
using WeatherForecast.ApiResponseConverters.DataConverters;

namespace WeatherForecast.UnitTests.ApiResponseConvertersTests
{
    [TestFixture]
    public class IconConverterTests
    {
        [TestCase("fake", "")]
        [TestCase("", "fake")]
        [TestCase("", "")]
        public void IconConverter_Generate_EmptyInput(string iconStorageAddress, string icon)
        {
            //Arrange
            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => IconConverter.Generate(iconStorageAddress, icon), "Invalid input parameters.");
        }

        [TestCase("01n", "http://openweathermap.org/img/wn/iconcode@2x.png", ExpectedResult = "http://openweathermap.org/img/wn/01n@2x.png")]
        [TestCase("01d", "http://openweathermap.org/img/wn/iconcode@2x.png", ExpectedResult = "http://openweathermap.org/img/wn/01d@2x.png")]
        public string IconConverter_Generate_CorrectInput(string icon, string iconStorageAddress)
        {
            //Arrange
            //Act
            //Assert
            return IconConverter.Generate(iconStorageAddress, icon);
        }

        [TestCase("fake", "", "")]
        [TestCase("", "fake", "")]
        [TestCase("", "", "fake")]
        [TestCase("", "", "")]
        public void IconConverter_Generate2_EmptyInput(string iconStorageAddress, string icon, string daylightKey)
        {
            //Arrange
            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => IconConverter.Generate(iconStorageAddress, icon, daylightKey), "Invalid input parameters.");
        }

        [TestCase("http://openweathermap.org/img/wn/iconcode@2x.png", "01", "d", ExpectedResult = "http://openweathermap.org/img/wn/01d@2x.png")]
        public string IconConverter_Generate2_CorrectInput(string iconStorageAddress, string icon, string daylightKey)
        {
            //Arrange
            //Act
            //Assert
            return IconConverter.Generate(iconStorageAddress, icon, daylightKey);
        }

        [TestCase("fake", "", "")]
        [TestCase("", "fake", "")]
        [TestCase("", "", "fake")]
        [TestCase("", "", "")]
        public void IconConverter_CutDNSymbol_EmptyInput(string icon, string daylightKey, string nightKey)
        {
            //Arrange
            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => IconConverter.CutDNSymbol(icon, daylightKey, nightKey), "Invalid input parameters.");
        }

        [TestCase("01n", "d", "n", ExpectedResult = "01")]
        [TestCase("01d", "d", "n", ExpectedResult = "01")]
        public string IconConverter_CutDNSymbol_CorrectInput(string icon, string daylightKey, string nightKey)
        {
            //Arrange
            //Act
            //Assert
            return IconConverter.CutDNSymbol(icon, daylightKey, nightKey);
        }
    }
}
