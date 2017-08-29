using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeatherMap.OpenWeatherMap.RestEntities
{
    class Weather
    {
        public Coordinate coord;
        public ICollection<WeatherCondition> weather;
        [JsonProperty("base")]
        public string baseStr;
        public WeatherData main;
        public Wind wind;
        public Cloud clouds;
        public Rain rain;
        public Snow snow;
        public DateTime dt;
        public Sys sys;
        public int id;
        public string name;
        public string cod;

        public class Wind
        {
            public float speed;
            public float deg;
        }

        public class WeatherData
        {
            public float temp;
            public float pressure;
            public float humidity;
            public float temp_min;
            public float temp_max;
            public float sea_level;
            public float grnd_level;
        }

        public class Cloud
        {
            public float all;
        }

        public class Rain
        {
            [JsonProperty("3h")]
            public float last3Hours;
        }

        public class Snow
        {
            [JsonProperty("3h")]
            public float last3Hours;
        }

        public class Sys
        {
            public int type;
            public int id;
            public float message;
            public string country;
            public DateTime sunrise;
            public DateTime sunset;
        }
    }
}
