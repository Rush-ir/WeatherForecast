namespace WeatherForecast.Interfaces
{
    /// <summary>
    /// Interface for methods to convert from response entity to local model
    /// </summary>
    /// <typeparam name="T">External api model</typeparam>
    /// <typeparam name="K">Local model</typeparam>
    public interface IExternalApiResponseConverter<T, K> where T : class where K : class
    {
        K Convert(T externalApiModel);
    }
}

