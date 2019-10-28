using System;

namespace WeatherForecast.Entities.ConfigurationSections
{
    /// <summary>
    /// Class containing pollicy settings
    /// </summary>
    public class PollySettings
    {
        public HttpCircuitBreaker HttpCircuitBreaker { get; set; }

        public HttpRetry HttpRetry { get; set; }

        public Cache Cache { get; set; }
    }

    public class HttpCircuitBreaker
    {
        public TimeSpan DurationOfBreak { get; set; }

        public int ExceptionsAllowedBeforeBreaking { get; set; }
    }

    public class HttpRetry
    {
        public int AmountOfRetries { get; set; }

        public TimeSpan Delay { get; set; }
    }

    public class Cache
    {
        //Lifetime is an amount of minutes after which object is removed from cache
        public int Lifetime { get; set; }
    }
}
