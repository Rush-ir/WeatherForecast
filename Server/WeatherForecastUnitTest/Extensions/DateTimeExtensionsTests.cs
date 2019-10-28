using NUnit.Framework;
using System;
using WeatherForecast.Extensions;

namespace WeatherForecast.UnitTests.Extensions
{
    [TestFixture]
    public class DateTimeExtensionsTests
    {
        [Test]
        public void ToWeatherDateTimeFormat()
        {
            //Arrange
            DateTime dt = new DateTime(2019, 10, 27, 6, 0, 0);
            int offset = 3600;
            var expectedResult = "07:00 27 October";

            //Act
            var result = dt.ToWeatherDateTimeFormat(offset);

            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void ToWeatherDateFormat()
        {
            //Arrange
            DateTime dt = new DateTime(2019, 10, 27, 6, 0, 0);
            int offset = 3600;
            var expectedResult = "27 October";

            //Act
            var result = dt.ToWeatherDateFormat(offset);

            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void ToWeatherTimeFormat()
        {
            //Arrange
            DateTime dt = new DateTime(2019, 10, 27, 6, 0, 0);
            int offset = 3600;
            var expectedResult = "07:00";

            //Act
            var result = dt.ToWeatherTimeFormat(offset);

            //Assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}
