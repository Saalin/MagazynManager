using MagazynManager.Domain.Entities.Uzytkownicy;
using MagazynManager.Domain.Specification.Specifications;
using MagazynManager.Infrastructure.InputModel.Authentication;
using MagazynManager.Tests.ObjectMothers;
using Newtonsoft.Json;
using NodaTime;
using NodaTime.Testing;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static MagazynManager.Tests.Technical.JsonSerializerUtils;

namespace MagazynManager.Tests.IntegrationTests
{
    [TestFixture]
    public class LoginTests : AuthorizedTest
    {
        [Test]
        public void Test_User_Registered()
        {
            var service = (IUserRepository)_factory.Services.GetService(typeof(IUserRepository));
            var userDto = service.GetUser(new EmailSpecification("admin@admin.com"), UserLoginModel.PrzedsiebiorstwoId);
            Assert.That(userDto, Is.Not.Null);
            Assert.That(userDto.Email, Is.EqualTo(UserLoginModel.Email));
        }

        [Test]
        public async Task Try_Access_Non_Existing_Uri()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/non-existing-page");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task Get_EndpointsReturnUnauthorizedAfterExpiration()
        {
            // Arrange
            var client = _factory.CreateClient();
            var service = (FakeClock)_factory.Services.GetService(typeof(IClock));

            //Act
            var tokens = await Authenticate(client).ConfigureAwait(false);
            service.AdvanceDays(60);

            var content = new StringContent(JsonConvert.SerializeObject(tokens, GetNodaTimeSerializerSettings()), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("user/refresh", content).ConfigureAwait(false);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task TestBadLogin()
        {
            var client = _factory.CreateClient();
            var badLoginModel = LoginObjectMother.GetBadLoginModel();

            var content = new StringContent(JsonConvert.SerializeObject(badLoginModel, GetNodaTimeSerializerSettings()), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("user/login", content).ConfigureAwait(false);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}