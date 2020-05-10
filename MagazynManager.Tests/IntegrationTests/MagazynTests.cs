using MagazynManager.Tests.IntegrationTests.ApiCallers;
using MagazynManager.Tests.ObjectMothers;
using NUnit.Framework;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MagazynManager.Tests.IntegrationTests
{
    [TestFixture]
    public class MagazynTests : AuthorizedTest
    {
        [Test]
        public async Task Add_Magazyn_And_Check_Count()
        {
            // Arrange
            var client = _factory.CreateClient();
            var magazynApiCaller = new MagazynApiCaller(client);

            var tokens = await Authenticate(client).ConfigureAwait(false);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.Token);

            await magazynApiCaller.DodajMagazyn(MagazynObjectMother.GetMagazyn());

            var magazyny = await magazynApiCaller.GetMagazyny();

            Assert.That(magazyny, Is.Not.Null);
            Assert.That(magazyny, Has.Count.EqualTo(1));
        }
    }
}