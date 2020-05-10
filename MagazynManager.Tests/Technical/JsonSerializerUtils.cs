using Newtonsoft.Json;
using NodaTime;
using NodaTime.Serialization.JsonNet;

namespace MagazynManager.Tests.Technical
{
    public static class JsonSerializerUtils
    {
        public static JsonSerializerSettings GetNodaTimeSerializerSettings()
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
            return jsonSerializerSettings;
        }
    }
}