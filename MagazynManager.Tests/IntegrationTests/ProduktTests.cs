using MagazynManager.Tests.IntegrationTests.ApiCallers;
using MagazynManager.Tests.ObjectMothers;
using NUnit.Framework;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MagazynManager.Tests.IntegrationTests
{
    [TestFixture]
    public class ProduktTests : AuthorizedTest
    {
        [Test]
        public async Task Add_Produkt_And_Check_Count()
        {
            // Arrange
            var client = _factory.CreateClient();
            var apiCaller = new ProduktApiCaller(client);

            var tokens = await Authenticate(client).ConfigureAwait(false);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.Token);

            var magazynId = await new MagazynApiCaller(client).DodajMagazyn(MagazynObjectMother.GetMagazyn());
            await apiCaller.DodajProdukt(ProduktObjectMother.GetProdukt(magazynId));

            var categories = await apiCaller.GetProdukty();

            Assert.That(categories, Is.Not.Null);
            Assert.That(categories, Has.Count.EqualTo(1));
        }
    }
}