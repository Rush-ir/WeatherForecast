using System.Collections.Generic;

namespace WeatherForecast.Entities.OpenWeatherApiEntities.OpenWeatherJsonEntities
{
    public class OpenWeather
    {
        //ticks
        public ulong Dt { get; set; }

        //Shift in seconds from UTC
        public ushort Timezone { get; set; }

        public IEnumerable<WeatherDetails> Weather { get; set; }

        public Main Main { get; set; }

        public Wind Wind { get; set; }
    }
}
