using MagazynManager.Domain.Entities.StukturaOrganizacyjna;
using MagazynManager.Infrastructure.InputModel.Slowniki;
using MagazynManager.Tests.Technical;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagazynManager.Tests.IntegrationTests.ApiCallers
{
    public class ProduktApiCaller : ApiCaller
    {
        public ProduktApiCaller(HttpClient client) : base(client)
        {
        }

        public Task<List<Magazyn>> GetProdukty()
        {
            return HttpClient.Get<List<Magazyn>>("produkt/list");
        }

        public Task<Guid> DodajProdukt(ProduktCreateModel model)
        {
            return HttpClient.Post<Guid>("produkt", model);
        }
    }
}