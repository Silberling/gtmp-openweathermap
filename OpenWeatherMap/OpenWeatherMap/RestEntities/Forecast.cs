using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeatherMap.OpenWeatherMap.RestEntities
{
    class Forecast
    {
#pragma warning disable 0649
        public string cod;
        public float message;
        public City city;
        public int cnt;
        public ICollection<ForecastEntry> list;

        public class City
        {
#pragma warning disable 0649
            public int id;
            public string name;
            public Coordinate coord;
            public string country;
        }
    }

    public class ForecastEntry
    {
#pragma warning disable 0649
        public DateTime dt;
        public TemperatureEntry temp;
        public float pressure;
        public float humidity;
        public ICollection<WeatherCondition> weather;
        public float speed;
        public float deg;
        public float clouds;

        public class TemperatureEntry
        {
#pragma warning disable 0649
            public float day;
            public float min;
            public float max;
            public float night;
            public float eve;
            public float morn;
        }
    }
}
