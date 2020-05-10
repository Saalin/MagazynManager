using MagazynManager.Domain.Entities.Dokumenty;
using MagazynManager.Infrastructure.InputModel.Ewidencja;
using MagazynManager.Tests.Technical;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagazynManager.Tests.IntegrationTests.ApiCallers
{
    public class PrzyjecieApiCaller : ApiCaller
    {
        public PrzyjecieApiCaller(HttpClient client) : base(client)
        {
        }

        public Task<List<Dokument>> GetDokumentyPrzyjecia()
        {
            return HttpClient.Get<List<Dokument>>("przyjecie/list");
        }

        public Task<Guid> Przyjmij(PrzyjecieCreateModel model)
        {
            return HttpClient.Post<Guid>("przyjecie/Przyjmij", model);
        }
    }
}