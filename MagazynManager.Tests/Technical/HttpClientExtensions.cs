using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static MagazynManager.Tests.Technical.JsonSerializerUtils;

namespace MagazynManager.Tests.Technical
{
    public static class HttpClientExtensions
    {
        public async static Task<T> Get<T>(this HttpClient client, string uri)
        {
            var serializerSettings = GetNodaTimeSerializerSettings();
            var getResponse = await client.GetAsync(uri).ConfigureAwait(false);
            var listContnet = await getResponse.Content.ReadAsStringAsync();

            try
            {
                return JsonConvert.DeserializeObject<T>(listContnet, serializerSettings);
            }
            catch (JsonSerializationException)
            {
                throw new Exception(listContnet);
            }
        }

        public async static Task<T> Post<T>(this HttpClient client, string uri, object obj)
        {
            var serializerSettings = GetNodaTimeSerializerSettings();
            var content = new StringContent(JsonConvert.SerializeObject(obj, serializerSettings), Encoding.UTF8, "application/json");
            var result = await client.PostAsync(uri, content).ConfigureAwait(false);

            var contentString = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

            try
            {
                return JsonConvert.DeserializeObject<T>(contentString, serializerSettings);
            }
            catch (JsonSerializationException)
            {
                throw new Exception(contentString);
            }
        }

        public async static Task Post(this HttpClient client, string uri, object obj)
        {
            var serializerSettings = GetNodaTimeSerializerSettings();
            var content = new StringContent(JsonConvert.SerializeObject(obj, serializerSettings), Encoding.UTF8, "application/json");
            await client.PostAsync(uri, content).ConfigureAwait(false);
        }
    }
}