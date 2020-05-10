using MagazynManager.Infrastructure.Dto.Ewidencja;
using MagazynManager.Tests.Technical;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagazynManager.Tests.IntegrationTests.ApiCallers
{
    public class StanAktualnyApiCaller : ApiCaller
    {
        public StanAktualnyApiCaller(HttpClient client) : base(client)
        {
        }

        public Task<List<StanAktualnyDto>> GetStanAktualny(Guid magazynId)
        {
            return HttpClient.Get<List<StanAktualnyDto>>($"stan/AktualneStanyList/{magazynId}");
        }
    }
}