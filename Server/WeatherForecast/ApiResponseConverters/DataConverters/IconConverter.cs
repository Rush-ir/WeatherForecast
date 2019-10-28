using System;
using System.Text.RegularExpressions;

namespace WeatherForecast.ApiResponseConverters.DataConverters
{
    /// <summary>
    /// Contains methods to convert icon name to appropriate format
    /// </summary>
    public static class IconConverter
    {
        /// <summary>
        /// Generates full icon url
        /// </summary>
        /// <param name="iconStorageAddress">Property from appsettings.json</param>
        /// <param name="icon">Icon name</param>
        /// <returns></returns>
        public static string Generate(string iconStorageAddress, string icon)
        {
            if (string.IsNullOrEmpty(iconStorageAddress) || string.IsNullOrEmpty(icon))
            {
                throw new ArgumentNullException("Invalid input parameters.");
            }

            return iconStorageAddress.Replace("iconcode", icon);
        }

        /// <summary>
        /// Generates full icon url for daylight
        /// </summary>
        /// <param name="iconStorageAddress">Property from appsettings.json</param>
        /// <param name="icon">Icon name without daylight/night symbol</param>
        /// <param name="daylightKey">Property from appsettings.json. Shows if icon is made for daylight.</param>
        /// <param name="nightKey">Property from appsettings.json. Shows if icon is made for night.</param>
        /// <returns></returns>
        public static string Generate(string iconStorageAddress, string icon, string daylightKey)
        {
            if (string.IsNullOrEmpty(iconStorageAddress) 
                || string.IsNullOrEmpty(icon)
                || string.IsNullOrEmpty(daylightKey))
            {
                throw new ArgumentNullException("Invalid input parameters.");
            }
            return iconStorageAddress.Replace("iconcode", icon + daylightKey);
        }

        /// <summary>
        /// Remove icon daylight/night symbol
        /// </summary>
        /// <param name="icon">Icon name</param>
        /// <param name="daylightKey">Property from appsettings.json. Shows if icon is made for daylight.</param>
        /// <param name="nightKey">Property from appsettings.json. Shows if icon is made for night.</param>
        /// <returns></returns>
        public static string CutDNSymbol(string icon, string daylightKey, string nightKey)
        {
            if (string.IsNullOrEmpty(nightKey)
                || string.IsNullOrEmpty(icon)
                || string.IsNullOrEmpty(daylightKey))
            {
                throw new ArgumentNullException("Invalid input parameters.");
            }

            return Regex.Replace(icon, $"{nightKey}|{daylightKey}", "");
        }
    }
}
