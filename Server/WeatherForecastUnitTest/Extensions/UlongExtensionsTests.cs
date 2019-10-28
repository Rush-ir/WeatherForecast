using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using WeatherForecast.Extensions;

namespace WeatherForecast.UnitTests.Extensions
{
    [TestFixture]
    public class UlongExtensionsTests
    {
        [Test]
        public void ToDateTime()
        {
            //Arrange
            ulong tm = 1572156000; //Sunday, 27 October 2019, 6:00:00
            DateTime expectedResult = new DateTime(2019, 10, 27, 6, 0, 0);

            //Act
            var result = tm.ToDateTime();

            //Assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}
