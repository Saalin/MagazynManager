using MagazynManager.Infrastructure.InputModel.Ewidencja;
using MagazynManager.Tests.Technical;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagazynManager.Tests.IntegrationTests.ApiCallers
{
    public class WydanieApiCaller : ApiCaller
    {
        public WydanieApiCaller(HttpClient client) : base(client)
        {
        }

        public Task<Guid> Wydaj(WydanieCreateModel model)
        {
            return HttpClient.Post<Guid>("Wydanie/Wydaj", model);
        }
    }
}