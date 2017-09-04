using Newtonsoft.Json;
using OpenWeatherMap.OpenWeatherMap.RestEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeatherMap.OpenWeatherMap
{
    class OpenWeatherMap
    {
        private string appId;
        private HttpClient client;

        public OpenWeatherMap(string appId)
        {
            this.appId = appId;

            client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Weather> GetCurrentWeather(int locationId, string units)
        {
            Task<string> result = client.GetStringAsync("http://api.openweathermap.org/data/2.5/weather?id=" + locationId + "&appid=" + appId + "&units=" + units);
            string json = await result;
            return JsonConvert.DeserializeObject<Weather>(json, Globals.DefaultJsonSerializerSettings);
        }

        public async Task<Forecast> GetDailyForecast(int locationId, string units)
        {
            Task<string> result = client.GetStringAsync("http://api.openweathermap.org/data/2.5/forecast/daily?id=" + locationId + "&appid=" + appId + "&units=" + units);
            string json = await result;
            return JsonConvert.DeserializeObject<Forecast>(json, Globals.DefaultJsonSerializerSettings);
        }
    }
}
