using MagazynManager.Domain.Entities.Kontrahent;
using MagazynManager.Infrastructure.InputModel.Slowniki;
using MagazynManager.Tests.Technical;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagazynManager.Tests.IntegrationTests.ApiCallers
{
    public class KontrahentApiCaller : ApiCaller
    {
        public KontrahentApiCaller(HttpClient client) : base(client)
        {
        }

        public Task<List<Kontrahent>> GetList()
        {
            return HttpClient.Get<List<Kontrahent>>("Kontrahent/list");
        }

        public Task<Guid> DodajKontrahenta(KontrahentCreateModel model)
        {
            return HttpClient.Post<Guid>("Kontrahent", model);
        }
    }
}