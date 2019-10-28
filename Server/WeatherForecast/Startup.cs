using System;
using System.Net.Http;
using CorrelationId;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Caching;
using Polly.Caching.Memory;
using Polly.Registry;
using WeatherForecast.ApiResponseConverters;
using WeatherForecast.BusinessLogic;
using WeatherForecast.Entities.ConfigurationSections;
using WeatherForecast.Entities.Models;
using WeatherForecast.Entities.OpenWeatherApiEntities.OpenWeatherJsonEntities;
using WeatherForecast.Extensions;
using WeatherForecast.Interfaces;
using WeatherForecast.JsonEntities.OpenWeatherForecast;
using WeatherForecast.Retrievers;

namespace WeatherForecast
{
    public class Startup
    {
        private IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IForecastProvider, ForecastProvider>();
            services.AddTransient<IWeatherProvider, WeatherProvider>();
            services.AddTransient<IApiDataRetriever<OpenWeatherForecast>, WeatherForecastApiRetriever<OpenWeatherForecast>>();
            services.AddTransient<IApiDataRetriever<OpenWeather>, WeatherForecastApiRetriever<OpenWeather>>();
            services.AddSingleton<IExternalApiUriProvider, OpenWeatherForecastUriProvider>();
            services.AddSingleton<IExternalApiResponseConverter<OpenWeatherForecast, ForecastModel>, ForecastResponseConverter>();
            services.AddSingleton<IExternalApiResponseConverter<OpenWeather, CurrentWeatherModel>, WeatherResponseConverter>();

            services.AddControllers();

            services.AddLogging();

            //configure settings section to get weather forecast api details from appsettings file
            var weatherForecastProviderSettingsSection = _configuration.GetSection("WeatherForecastProviderSettings");
            services.Configure<WeatherForecastProviderSettings>(weatherForecastProviderSettingsSection);
            var weatherForecastProviderSettings = weatherForecastProviderSettingsSection.Get<WeatherForecastProviderSettings>();

            //configure settings section to get polly settings from appsettings file
            var pollySettingsSection = _configuration.GetSection("PollySettings");
            services.Configure<PollySettings>(pollySettingsSection);
            var pollySettings = pollySettingsSection.Get<PollySettings>();

            services.AddCorrelationId();
            //to cache responses from external api
            services.AddMemoryCache();
            services.AddSingleton<IAsyncCacheProvider, MemoryCacheProvider>();

            services.AddSingleton<IReadOnlyPolicyRegistry<string>, PolicyRegistry>((serviceProvider) =>
            {
                PolicyRegistry registry = new PolicyRegistry();
                registry.Add("WeatherForecastCachePolicy",
                    Policy.CacheAsync(
                        serviceProvider
                            .GetRequiredService<IAsyncCacheProvider>()
                            .AsyncFor<HttpResponseMessage>(),
                        TimeSpan.FromMinutes(pollySettings.Cache.Lifetime)));
                return registry;
            });

            //register IHttpClientFactory
            services.AddHttpClient(
                    weatherForecastProviderSettings.ProviderName,
                    client =>
                    {
                        client.BaseAddress = new Uri(weatherForecastProviderSettings.BaseAddress);
                        client.DefaultRequestHeaders.Add("Accept", "application/json");

                        client.Timeout = weatherForecastProviderSettings.Timeout;
                    })
                .SetHandlerLifetime(TimeSpan.FromMinutes(10))
                .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(
                    pollySettings.HttpRetry.AmountOfRetries,
                    i => pollySettings.HttpRetry.Delay))
                .AddTransientHttpErrorPolicy(builder => builder.CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: pollySettings.HttpCircuitBreaker.ExceptionsAllowedBeforeBreaking,
                    durationOfBreak: pollySettings.HttpCircuitBreaker.DurationOfBreak
                ));

            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin",
                    builder => builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
                                .WithMethods("GET")
                );
            });
        }

        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env,  
            ILoggerFactory loggerFactory)
        {
            loggerFactory.AddLog4Net();            

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowOrigin");

            app.UseExceptionMiddleware();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
