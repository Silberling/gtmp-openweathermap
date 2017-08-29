using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeatherMap
{
    public enum GtaWeatherStates
    {
        Unspecified = -1,

        ExtraSunny = 0,
        Clear = 1,
        Clouds = 2,
        Smog = 3,
        Foggy = 4,
        Overcast = 5,
        Rain = 6,
        Thunder = 7,
        LightRain = 8,
        SmoggyLightRain = 9,
        VeryLightSnow = 10,
        WindyLightSnow = 11,
        LightSnow = 12,
        Unknown = 13
    }
}
