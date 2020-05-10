using MagazynManager.Infrastructure.Dto.Slowniki;
using MagazynManager.Infrastructure.InputModel.Slowniki;
using MagazynManager.Tests.Technical;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagazynManager.Tests.IntegrationTests.ApiCallers
{
    public class JednostkaMiaryApiCaller : ApiCaller
    {
        public JednostkaMiaryApiCaller(HttpClient client) : base(client)
        {
        }

        public Task<List<JednostkaMiaryDto>> GetJednostkiMiary()
        {
            return HttpClient.Get<List<JednostkaMiaryDto>>("JednostkaMiary/list");
        }

        public Task<Guid> AddJednostkaMiary(JednostkaMiaryCreateModel model)
        {
            return HttpClient.Post<Guid>("JednostkaMiary", model);
        }
    }
}