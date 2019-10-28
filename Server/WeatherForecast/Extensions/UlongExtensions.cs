using System;

namespace WeatherForecast.Extensions
{
    /// <summary>
    /// Extentions for data type ulong
    /// </summary>
    public static class UlongExtensions
    {
        /// <summary>
        /// Converts a timestamp to DateTime object.
        /// </summary>
        /// <param name="timeStamp">number of seconds since the Epoch</param>
        /// <returns cref="DateTime"></returns>
        public static DateTime ToDateTime(this ulong timeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            // Add the timestamp
            return dateTime.AddSeconds(timeStamp);
        }
    }
}
