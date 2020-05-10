using MagazynManager.Tests.IntegrationTests.ApiCallers;
using MagazynManager.Tests.ObjectMothers;
using NUnit.Framework;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MagazynManager.Tests.IntegrationTests
{
    public class KontrahentTest : AuthorizedTest
    {
        [Test]
        public async Task Add_Kontrahent_And_Check_Count()
        {
            // Arrange
            var client = _factory.CreateClient();
            var apiCaller = new KontrahentApiCaller(client);

            var tokens = await Authenticate(client).ConfigureAwait(false);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.Token);

            await apiCaller.DodajKontrahenta(KontrahentObjectMother.GetKontrahent());

            var categories = await apiCaller.GetList();

            Assert.That(categories, Is.Not.Null);
            Assert.That(categories, Has.Count.EqualTo(1));
        }
    }
}