using MagazynManager.Infrastructure.Dto.Slowniki;
using MagazynManager.Infrastructure.InputModel.Slowniki;
using MagazynManager.Tests.Technical;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagazynManager.Tests.IntegrationTests.ApiCallers
{
    public class KategoriaApiCaller : ApiCaller
    {
        public KategoriaApiCaller(HttpClient client) : base(client)
        {
        }

        public Task<List<KategoriaDto>> GetKategorieList()
        {
            return HttpClient.Get<List<KategoriaDto>>("kategoria/list");
        }

        public Task<Guid> DodajKategorie(KategoriaCreateModel model)
        {
            return HttpClient.Post<Guid>("kategoria", model);
        }

        public Task<HttpResponseMessage> UsunKategorie(Guid id)
        {
            return HttpClient.DeleteAsync($"kategoria/{id}");
        }
    }
}