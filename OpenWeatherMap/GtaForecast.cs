using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeatherMap
{
    class GtaForecast
    {
        public ICollection<GtaForecastEntry> entries;

        public GtaForecast()
        {
            entries = new List<GtaForecastEntry>();
        }
    }

    class GtaForecastEntry
    {
        public GtaWeatherStates state;
        public float dayTemp;
        public float nightTemp;
        public DateTime? dateTime;

        public GtaForecastEntry(GtaWeatherStates state, float dayTemp, float nightTemp, DateTime? dateTime)
        {
            this.state = state;
            this.dayTemp = dayTemp;
            this.nightTemp = nightTemp;
            this.dateTime = dateTime;
        }
    }
}
