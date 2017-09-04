using GrandTheftMultiplayer.Server.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeatherMap
{
    class Settings
    {
        public string OWMAppId { get; private set; }
        public int OWMLocationId { get; private set; }
        public int OWMUpdateInterval { get; private set; }
        public string OWMUnits { get; private set; }

        public Settings(API API)
        {
            OWMAppId = API.getSetting<string>("owm_appId");
            OWMLocationId = Int32.Parse(API.getSetting<string>("owm_locationId"));
            OWMUpdateInterval = Int32.Parse(API.getSetting<string>("owm_updateInterval"));
            OWMUnits = API.getSetting<string>("owm_units");
        }
    }
}
