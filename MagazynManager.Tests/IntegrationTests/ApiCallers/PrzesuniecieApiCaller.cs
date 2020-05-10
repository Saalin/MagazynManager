using MagazynManager.Infrastructure.InputModel.Ewidencja;
using MagazynManager.Tests.Technical;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagazynManager.Tests.IntegrationTests.ApiCallers
{
    public class PrzesuniecieApiCaller : ApiCaller
    {
        public PrzesuniecieApiCaller(HttpClient client) : base(client)
        {
        }

        public Task<Guid> Przesun(PrzesuniecieCreateModel model)
        {
            return HttpClient.Post<Guid>("Przesuniecie/Przesun", model);
        }
    }
}