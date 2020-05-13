using MagazynManager.Tests.IntegrationTests.ApiCallers;
using MagazynManager.Tests.ObjectMothers;
using NUnit.Framework;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MagazynManager.Tests.IntegrationTests
{
    [TestFixture]
    public class DokumentWydaniaTests : AuthorizedTest
    {
        [Test]
        public async Task CheckStanAktualnyAfterPrzesuniecie()
        {
            // Arrange
            var client = _factory.CreateClient();

            var tokens = await Authenticate(client).ConfigureAwait(false);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.Token);

            var magazynId = await new MagazynApiCaller(client).DodajMagazyn(MagazynObjectMother.GetMagazyn());
            var produktId = await new ProduktApiCaller(client).DodajProdukt(ProduktObjectMother.GetProdukt(magazynId));

            var dokumentPrzyjecia = DokumentObjectMother.GetDokumentPrzyjeciaZJednaPozycja(magazynId, produktId, 10);
            await new PrzyjecieApiCaller(client).Przyjmij(dokumentPrzyjecia);

            var wydanieModel = DokumentObjectMother.GetDokumentWydaniaZJednaPozycja(magazynId, produktId, 7);
            await new WydanieApiCaller(client).Wydaj(wydanieModel);

            var stanyWydane = await new StanAktualnyApiCaller(client).GetStanAktualny(magazynId);
            Assert.That(stanyWydane, Has.Count.EqualTo(1));
            Assert.That(stanyWydane.First().Ilosc, Is.EqualTo(3));
        }
    }
}