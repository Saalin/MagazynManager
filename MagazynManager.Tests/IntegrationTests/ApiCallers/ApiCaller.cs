using System.Net.Http;

namespace MagazynManager.Tests.IntegrationTests.ApiCallers
{
    public abstract class ApiCaller
    {
        protected HttpClient HttpClient;

        protected ApiCaller(HttpClient client)
        {
            HttpClient = client;
        }
    }
}