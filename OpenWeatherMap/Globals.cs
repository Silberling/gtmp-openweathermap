using Newtonsoft.Json;

namespace OpenWeatherMap
{
    class Globals
    {
        public static JsonSerializerSettings DefaultJsonSerializerSettings = new JsonSerializerSettings {
            Formatting = Formatting.None,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore
        };
    }
}
