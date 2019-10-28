namespace WeatherForecast.Entities.OpenWeatherApiEntities.OpenWeatherJsonEntities
{
    /// <summary>
    /// External api entity
    /// Contains main weather conditions
    /// </summary>
    public class Main
    {
        /// <summary>
        /// Humidity
        /// </summary>
        public int Humidity { get; set; }

        /// <summary>
        /// Pressure
        /// </summary>
        public int Pressure { get; set; }

        /// <summary>
        /// Temperature
        /// </summary>
        public float Temp { get; set; }
    }
}
