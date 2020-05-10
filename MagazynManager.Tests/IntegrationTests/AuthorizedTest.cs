using MagazynManager.Application.DataProviders;
using MagazynManager.Infrastructure.InputModel.Authentication;
using MagazynManager.Tests.IntegrationTests.ApiCallers;
using MagazynManager.Tests.ObjectMothers;
using MagazynManager.Tests.Technical;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagazynManager.Tests.IntegrationTests
{
    public abstract class AuthorizedTest : IntegrationTest<TestStartup>
    {
        protected UserLoginModel UserLoginModel;

        [SetUp]
        public void LoginUser()
        {
            UserLoginModel = LoginObjectMother.GetAdminLoginModel();
        }

        protected async Task<AuthResult> Authenticate(HttpClient httpClient)
        {
            return await new UserApiCaller(httpClient).Login(UserLoginModel);
        }
    }
}