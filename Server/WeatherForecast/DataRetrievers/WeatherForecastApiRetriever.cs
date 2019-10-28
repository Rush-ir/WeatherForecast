using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Registry;
using WeatherForecast.Entities.ConfigurationSections;
using WeatherForecast.Exceptions;
using WeatherForecast.Extensions;
using WeatherForecast.Interfaces;

namespace WeatherForecast.Retrievers
{
    /// <summary>
    /// Generic class designed to send request to an external api to get some data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WeatherForecastApiRetriever<T>: IApiDataRetriever<T> where T: class
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly WeatherForecastProviderSettings _weatherForecastProviderSettings;
        private readonly ILogger<WeatherForecastApiRetriever<T>> _logger;
        private readonly IReadOnlyPolicyRegistry<string> _policyRegistry;

        public WeatherForecastApiRetriever(
            ILogger<WeatherForecastApiRetriever<T>> logger,
            IHttpClientFactory clientFactory,            
            IOptions<WeatherForecastProviderSettings> weatherForecastProviderSettingsAccessor,
            IReadOnlyPolicyRegistry<string> policyRegistry)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _weatherForecastProviderSettings = weatherForecastProviderSettingsAccessor.Value;
            _policyRegistry = policyRegistry;
        }

        /// <summary>
        /// Async method to send GET request
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="cacheContextName">Name of the Polly cache context</param>
        /// <returns><typeparam name="T"></typeparam></returns>
        public async Task<T> GetData(Uri requestUri, string cacheContextName) 
        {
            if (requestUri == null || String.IsNullOrEmpty(cacheContextName))
            {
                throw new ArgumentNullException("Invalid input parameters.");
            }

            try
            {                
                //generate hhtp client from httpfactory
                var httpClient = CreateHttpClient();
                if (httpClient != null)
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);

                    //retrieve caching policy
                    var _cachePolicy = _policyRegistry.Get<IAsyncPolicy<HttpResponseMessage>>("WeatherForecastCachePolicy");

                    //send request considering that response can be already cached
                    //if such request has already been sent, then response is returned from cache
                    //if such response is not in cache anymore, usual GET request is sent
                    var response = await _cachePolicy.ExecuteAsync(
                        async context => await httpClient.SendAsync(request), 
                        new Context($"{cacheContextName}-{requestUri.Query}"));

                    //check response status
                    await response.EnsureSuccessStatusCodeAsync();
                    
                    var results = await response.Content?.ReadAsStringAsync();

                    //deserialize result
                    return JsonSerializer.Deserialize<T>(results, new JsonSerializerOptions
                    {
                        AllowTrailingCommas = true,
                        PropertyNameCaseInsensitive = true,

                    });
                }
                else
                {
                    _logger.LogError("Something happened during HttpClient creation.");

                    throw new HttpResponseWithStatusCodeException(HttpStatusCode.InternalServerError,
                        "Something went wrong. Please try later or contact admin.");
                }
            }
            catch (HttpResponseWithStatusCodeException ex)
            {
                _logger.LogError($"ApiMessage:{ex.ApiMessage}", ex);
                throw ex;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new HttpResponseWithStatusCodeException(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        #region private methods

        private HttpClient CreateHttpClient()
        {
            return _clientFactory.CreateClient(_weatherForecastProviderSettings.ProviderName);
        }

        #endregion
    }
}
