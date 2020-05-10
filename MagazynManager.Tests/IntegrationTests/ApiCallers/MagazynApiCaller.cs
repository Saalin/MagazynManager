using MagazynManager.Domain.Entities.StukturaOrganizacyjna;
using MagazynManager.Infrastructure.InputModel.Slowniki;
using MagazynManager.Tests.Technical;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagazynManager.Tests.IntegrationTests.ApiCallers
{
    public class MagazynApiCaller : ApiCaller
    {
        public MagazynApiCaller(HttpClient client) : base(client)
        {
        }

        public Task<List<Magazyn>> GetMagazyny()
        {
            return HttpClient.Get<List<Magazyn>>("magazyn/list");
        }

        public Task<Guid> DodajMagazyn(MagazynCreateModel model)
        {
            return HttpClient.Post<Guid>("magazyn", model);
        }
    }
}