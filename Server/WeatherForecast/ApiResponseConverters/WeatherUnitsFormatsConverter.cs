using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using WeatherForecast.Entities.OpenWeatherApiEntities;

namespace WeatherForecast.ApiResponseConverters
{
    /// <summary>
    /// Converts sensetive data (i.e temperature) accordigly to the specified units mode
    /// </summary>
    public static class WeatherUnitsFormatsConverter
    {
        private static readonly Dictionary<UnitsFormats, Dictionary<string, string>> _unitsSettings = new Dictionary<UnitsFormats, Dictionary<string, string>>
        {
            [UnitsFormats.Imperial] = new Dictionary<string, string> { ["TemperatureFormat"] = "F"},
            [UnitsFormats.Metric] = new Dictionary<string, string> { ["TemperatureFormat"] = "C" },
            [UnitsFormats.Standard] = new Dictionary<string, string> { ["TemperatureFormat"] = "K" }
        };

        /// <summary>
        /// Converts temperature accordigly to the specified units mode
        /// </summary>
        /// <param name="unitsFormat"></param>
        /// <returns></returns>
        public static string GetTemperatureFormat(UnitsFormats unitsFormat)
        {
            return _unitsSettings[unitsFormat]["TemperatureFormat"];
        }
    }
}
