using MetOfficeDataPoint.Models.GeoCoordinate;
using System;
using FreeGeoIPCore;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using WeatherApp.Models;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

namespace WeatherApp.Data
{
    public class WeatherForecastService
    {

        private static readonly string[] Cities = new[]
        {
            "Dhaka", "Chittagong", "London", "Delhi", "Sydney", "New York", "Dubai", "Sharjah", "Kathmandu", "Montreal"
        };


        public async Task<WeatherForecast[]> GetForecastAsync(string CityName="")
        {
            List<WeatherForecast> weatherList = new List<WeatherForecast>();
            if (CityName == "")
            { 

                foreach(string city in Cities)
                {
                    using (var httpClient = new HttpClient())
                    {
                        using (var response = await httpClient.GetAsync("http://api.openweathermap.org/data/2.5/weather?q=" + city + "&appid=" + "6b2f9b8f4a7b0da1608d80db34c8ceea&units=metric"))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            var obj= JsonConvert.DeserializeObject<dynamic>(apiResponse);
                            WeatherForecast weatherForecast = new WeatherForecast
                            {
                                City = city,
                                Date = DateTime.Now,
                                Summary = obj.weather[0].main,
                                TemperatureC = obj.main.temp,
                                Description = obj.weather[0].description,
                                FeelsLike = obj.main.feels_like
                            };
        
                            weatherList.Add(weatherForecast);
                            
                        }
                    }
                }

            }
            else
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("http://api.openweathermap.org/data/2.5/weather?q=" + CityName + "&appid=" + "6b2f9b8f4a7b0da1608d80db34c8ceea&units=metric"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var obj = JsonConvert.DeserializeObject<dynamic>(apiResponse);
                        if(obj.cod=="404")
                        {
                            WeatherForecast weatherForecast = new WeatherForecast
                            {
                                City = "City not found",
                                Date = DateTime.Now,
                                Summary = "",
                                TemperatureC = 0,
                                Description = "",
                                FeelsLike = 0
                            };

                            weatherList.Add(weatherForecast);
                        }
                        else
                        {
                            WeatherForecast weatherForecast = new WeatherForecast
                            {
                                City = CityName,
                                Date = DateTime.Now,
                                Summary = obj.weather[0].main,
                                TemperatureC = obj.main.temp,
                                Description = obj.weather[0].description,
                                FeelsLike = obj.main.feels_like
                            };

                            weatherList.Add(weatherForecast);
                        }
                        
                    }
                }
            }
            return weatherList.ToArray();

        }

       
    }
}
