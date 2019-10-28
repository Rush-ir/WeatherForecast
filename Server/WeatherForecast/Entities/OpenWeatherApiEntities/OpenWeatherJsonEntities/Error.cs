namespace WeatherForecast.Entities.OpenWeatherApiEntities.OpenWeatherJsonEntities
{
    /// <summary>
    /// Represents a structure returned in the case of external api failure
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Status code
        /// </summary>
        public string Cod { get; set; }

        /// <summary>
        /// Failure reason
        /// </summary>
        public string Message { get; set; }
    }
}
