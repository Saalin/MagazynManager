using MagazynManager.Tests.IntegrationTests.ApiCallers;
using MagazynManager.Tests.ObjectMothers;
using NUnit.Framework;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MagazynManager.Tests.IntegrationTests
{
    [TestFixture]
    public class DokumentPrzyjeciaTests : AuthorizedTest
    {
        [Test]
        public async Task Get_DokumentyPrzyjeciaListAfterAuthorization()
        {
            // Arrange
            var client = _factory.CreateClient();

            var tokens = await Authenticate(client).ConfigureAwait(false);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.Token);

            // Act
            var response = await new KategoriaApiCaller(client).GetKategorieList();

            Assert.That(response, Is.Not.Null);
        }

        [Test]
        public async Task Add_DokumentPrzyjecia_And_Check_Count()
        {
            // Arrange
            var client = _factory.CreateClient();
            var apiCaller = new PrzyjecieApiCaller(client);

            var tokens = await Authenticate(client).ConfigureAwait(false);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.Token);

            var magazynId = await new MagazynApiCaller(client).DodajMagazyn(MagazynObjectMother.GetMagazyn());
            var produktId = await new ProduktApiCaller(client).DodajProdukt(ProduktObjectMother.GetProdukt(magazynId));

            var przyjecieModel = DokumentObjectMother.GetDokumentPrzyjeciaZJednaPozycja(magazynId, produktId, 10);
            await apiCaller.Przyjmij(przyjecieModel);

            var dokumenty = await apiCaller.GetDokumentyPrzyjecia();

            Assert.That(dokumenty, Is.Not.Null);
            Assert.That(dokumenty, Has.Count.EqualTo(1));
        }

        [Test]
        public async Task CheckStanAktualnyAfterPrzyjecie()
        {
            // Arrange
            var client = _factory.CreateClient();
            var apiCaller = new PrzyjecieApiCaller(client);

            var tokens = await Authenticate(client).ConfigureAwait(false);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.Token);

            var magazynId = await new MagazynApiCaller(client).DodajMagazyn(MagazynObjectMother.GetMagazyn());
            var produktId = await new ProduktApiCaller(client).DodajProdukt(ProduktObjectMother.GetProdukt(magazynId));

            var przyjecieModel = DokumentObjectMother.GetDokumentPrzyjeciaZJednaPozycja(magazynId, produktId, 10);
            await apiCaller.Przyjmij(przyjecieModel);

            var stany = await new StanAktualnyApiCaller(client).GetStanAktualny(magazynId);

            Assert.That(stany, Has.Count.EqualTo(1));
        }
    }
}