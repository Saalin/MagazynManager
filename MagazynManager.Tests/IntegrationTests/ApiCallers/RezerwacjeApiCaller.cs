using MagazynManager.Infrastructure.Dto.Rezerwacje;
using MagazynManager.Infrastructure.InputModel.Rezerwacje;
using MagazynManager.Tests.Technical;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagazynManager.Tests.IntegrationTests.ApiCallers
{
    public class RezerwacjeApiCaller : ApiCaller
    {
        public RezerwacjeApiCaller(HttpClient client) : base(client)
        {
        }

        public Task<List<RezerwacjaDto>> GetList()
        {
            return HttpClient.Get<List<RezerwacjaDto>>("Rezerwacja/List");
        }

        public Task<Guid> Rezerwuj(RezerwacjaCreateModel model)
        {
            return HttpClient.Post<Guid>("Rezerwacja/Rezerwuj", model);
        }

        public async Task Anuluj(Guid id)
        {
            await HttpClient.DeleteAsync($"Rezerwacja/Anuluj/{id}");
        }
    }
}