using System;
using System.Globalization;

namespace WeatherForecast.Extensions
{
    /// <summary>
    /// DateTime extensions
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Convert datetime to date and time in timezone using offset and format it to string 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="offset">Shift in seconds from UTC</param>
        /// <returns>Date</returns>
        public static string ToWeatherDateTimeFormat(this DateTime dateTime, int offset)
        {
            return dateTime.AddSeconds(offset).ToString("HH:mm dd MMMM", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Convert datetime to timezone using offset and format it to string 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="offset">Shift in seconds from UTC</param>
        /// <returns>Date</returns>
        public static string ToWeatherDateFormat(this DateTime dateTime, int offset)
        {
            return dateTime.AddSeconds(offset).Date.ToString("dd MMMM", CultureInfo.InvariantCulture);
        }

        // <summary>
        /// Convert datetime to timezone using offset and format it to time string 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="offset">Shift in seconds from UTC</param>
        /// <returns>Time</returns>
        public static string ToWeatherTimeFormat(this DateTime dateTime, int offset)
        {
            return dateTime.AddSeconds(offset).ToString("HH:mm", CultureInfo.InvariantCulture);
        }
    }
}
