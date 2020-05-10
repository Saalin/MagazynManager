using MagazynManager.Application.DataProviders;
using MagazynManager.Infrastructure.InputModel.Authentication;
using MagazynManager.Tests.Technical;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagazynManager.Tests.IntegrationTests.ApiCallers
{
    public class UserApiCaller : ApiCaller
    {
        public UserApiCaller(HttpClient client) : base(client)
        {
        }

        public Task<AuthResult> RefreshToken(AuthResult credentials)
        {
            return HttpClient.Post<AuthResult>("user/refesh", credentials);
        }

        public Task<AuthResult> Login(UserLoginModel userLoginModel)
        {
            return HttpClient.Post<AuthResult>("user/login", userLoginModel);
        }
    }
}