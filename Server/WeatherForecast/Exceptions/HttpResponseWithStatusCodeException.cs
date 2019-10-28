using System;
using System.Net;

namespace WeatherForecast.Exceptions
{
    /// <summary>
    /// Custom exception which contains status code and api response message
    /// </summary>
    public class HttpResponseWithStatusCodeException: Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public string ApiMessage { get; set; }

        public HttpResponseWithStatusCodeException(HttpStatusCode statusCode, string message)
        {
            StatusCode = statusCode;
            ApiMessage = message;
        }
    }
}
