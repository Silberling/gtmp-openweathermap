using GrandTheftMultiplayer.Server.API;
using System;
using System.Linq;
using System.Threading.Tasks;
using GrandTheftMultiplayer.Server.Constant;
using OpenWeatherMap.OpenWeatherMap.RestEntities;
using OpenWeatherMap.Helper;
using System.Threading;
using System.Collections.Generic;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using Newtonsoft.Json;
using GrandTheftMultiplayer.Shared;

namespace OpenWeatherMap
{
    public class Main : Script
    {
        private OpenWeatherMap.OpenWeatherMap weather;

        Timer weatherTimer;
        Timer forecastTimer;

        GtaForecast forecast;

        Settings settings;

        public Main()
        {
            Globals.DefaultJsonSerializerSettings.Converters.Add(new UnixTimestampConverter());

            API.onResourceStart += OnResourceStart;
            API.onResourceStop += OnResourceStop;
            API.onClientEventTrigger += OnClientEventTrigger;
        }

        public void OnResourceStart()
        {
            settings = new Settings(API);

            weather = new OpenWeatherMap.OpenWeatherMap(settings.OWMAppId);

            weatherTimer = new Timer((e) =>
            {
                SyncWeather();
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(settings.OWMUpdateInterval));

            forecastTimer = new Timer((e) =>
            {
                GetForecast();
                //foreach (var i in forecast.entries)
                //{
                //    API.shared.consoleOutput(LogCat.Debug, "Forecast: " + i.state.ToString() + "(" + i.dayTemp + " > " + i.nightTemp + ") " + i.dateTime);
                //}
            }, null, TimeSpan.Zero, TimeSpan.FromHours(1));
        }

        private void OnResourceStop()
        {
            weatherTimer.Dispose();
            forecastTimer.Dispose();
        }

        //[System.Obsolete("DEMO COMMAND")]
        //[Command()]
        //public void GetForecast(Client client)
        //{
        //    API.shared.triggerClientEvent(client, "OWM_FORECAST", JsonConvert.SerializeObject(forecast, Globals.DefaultJsonSerializerSettings));
        //}

        private void OnClientEventTrigger(Client client, string eventName, object[] arguments)
        {
            switch(eventName)
            {
                case "OWM_GET_FORECAST":
                    API.shared.triggerClientEvent(client, "OWM_FORECAST", JsonConvert.SerializeObject(forecast, Globals.DefaultJsonSerializerSettings));
                    break;
            }
        }

        private void GetForecast()
        {
            Forecast nextForecast = GetCurrentLiveForecast().Result;

            GtaForecast forecast = new GtaForecast(settings.OWMUnits);

            foreach(ForecastEntry fe in nextForecast.list)
            {
                GtaWeatherStates nextWeather = FindFirstWeatherStateInConditionList(fe.weather);
                forecast.entries.Add(new GtaForecastEntry(nextWeather, fe.temp.day, fe.temp.night, fe.dt));
            }

            this.forecast = forecast;
        }

        private void SyncWeather()
        {
            Weather weather = GetCurrentLiveWeather().Result;

            GtaWeatherStates currentWeather = (GtaWeatherStates)Enum.ToObject(typeof(GtaWeatherStates), API.getWeather());
            GtaWeatherStates nextWeather = FindFirstWeatherStateInConditionList(weather.weather);
            if (nextWeather != GtaWeatherStates.Unspecified && currentWeather != nextWeather)
            {
                API.setWeather((int)nextWeather);
                //API.shared.consoleOutput(LogCat.Debug, "Updating weather: " + nextWeather.ToString());
            }
        }

        private GtaWeatherStates FindFirstWeatherStateInConditionList(ICollection<WeatherCondition> weatherConditionList)
        {
            IEnumerable<GtaWeatherStates> states = weatherConditionList.Select(wc => OpenWeatherMapWeatherIdToGtaEnum(wc.id)).Where(ws => ws != GtaWeatherStates.Unspecified);

            if (states.Count() > 0)
            {
                return states.First();
            }

            return GtaWeatherStates.Unspecified;
        }

        private GtaWeatherStates OpenWeatherMapWeatherIdToGtaEnum(int weatherId)
        {
            if (weatherId >= 200 && weatherId < 300)
            {
                return GtaWeatherStates.Thunder;
            }
            else if (weatherId >= 300 && weatherId < 400)
            {
                return GtaWeatherStates.LightRain;
            }
            else if (weatherId >= 500 && weatherId < 600)
            {
                return GtaWeatherStates.Rain;
            }
            else if (weatherId >= 600 && weatherId < 700)
            {
                return GtaWeatherStates.LightSnow;
            }
            else if (weatherId >= 700 && weatherId < 800)
            {
                if (weatherId == 701 || weatherId == 721 || weatherId == 741)
                {
                    return GtaWeatherStates.Foggy;
                }
                return GtaWeatherStates.Smog;
            }
            else if (weatherId == 800)
            {
                return GtaWeatherStates.Clear;
            }
            else if (weatherId > 800 && weatherId < 900)
            {
                if (weatherId >= 801 && weatherId <= 803)
                {
                    return GtaWeatherStates.Clouds;
                }
                return GtaWeatherStates.Overcast;
            }
            else if (weatherId >= 900 && weatherId < 950)
            {
                switch (weatherId)
                {
                    case 900: // Tornado (Thunderstorm)
                    case 901: // Tropical Storm (Thunderstorm)
                    case 902: // Hurricane (Thunderstorm)
                        return GtaWeatherStates.Thunder;
                    case 903: // Cold (Snow)
                        return GtaWeatherStates.WindyLightSnow;
                    case 904: // Hot (Clear)
                        return GtaWeatherStates.ExtraSunny;
                    case 905: // Windy (Slightly Covered)
                        return GtaWeatherStates.Clear;
                    case 906: // Hail (Rain)
                        return GtaWeatherStates.SmoggyLightRain;
                }
            }
            else if (weatherId >= 950 && weatherId <= 999) // Additional
            {
                switch (weatherId)
                {
                    case 951: // calm
                    case 952: // light breeze
                    case 953: // gentle breeze
                    case 954: // moderate breeze
                    case 955: // fresh breeze
                    case 956: // strong breeze
                        return GtaWeatherStates.Clear;
                    case 957: // high wind, near gale
                    case 958: // gale
                    case 959: // severe gale
                        return GtaWeatherStates.Clouds;
                    case 960: // storm
                    case 961: // violent storm
                    case 962: // hurricane
                        return GtaWeatherStates.Thunder;
                }
            }

            return GtaWeatherStates.Unspecified;
        }

        private async Task<Forecast> GetCurrentLiveForecast()
        {
            return await weather.GetDailyForecast(settings.OWMLocationId, settings.OWMUnits);
        }

        private async Task<Weather> GetCurrentLiveWeather()
        {
            return await weather.GetCurrentWeather(settings.OWMLocationId, settings.OWMUnits);
        }
    }
}
