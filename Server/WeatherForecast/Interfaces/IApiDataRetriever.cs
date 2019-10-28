using System;
using System.Threading.Tasks;

namespace WeatherForecast.Interfaces
{
    /// <summary>
    /// Interface to get data from external api by sending HTTP request
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IApiDataRetriever<T> where T: class
    {
        public Task<T> GetData(Uri requestUri, string cacheContextName);
    }
}
