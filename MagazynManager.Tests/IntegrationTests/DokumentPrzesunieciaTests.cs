using MagazynManager.Tests.IntegrationTests.ApiCallers;
using MagazynManager.Tests.ObjectMothers;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static MagazynManager.Tests.Technical.JsonSerializerUtils;

namespace MagazynManager.Tests.IntegrationTests
{
    [TestFixture]
    public class DokumentPrzesunieciaTests : AuthorizedTest
    {
        [Test]
        public async Task ProbaPrzesunieciaNieistniejacegoTowaru()
        {
            // Arrange
            var client = _factory.CreateClient();
            var magazynApiCaller = new MagazynApiCaller(client);

            var tokens = await Authenticate(client).ConfigureAwait(false);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.Token);

            var magazynWydaniaId = await magazynApiCaller.DodajMagazyn(MagazynObjectMother.GetMagazyn());
            var magazynPrzyjeciaId = await magazynApiCaller.DodajMagazyn(MagazynObjectMother.GetMagazyn());

            var produktId = await new ProduktApiCaller(client).DodajProdukt(ProduktObjectMother.GetProdukt(magazynWydaniaId));

            var dokumentPrzyjecia = DokumentObjectMother.GetDokumentPrzyjeciaZJednaPozycja(magazynWydaniaId, produktId, 10);
            await new PrzyjecieApiCaller(client).Przyjmij(dokumentPrzyjecia);

            var przesuniecieModel = DokumentObjectMother.GetPrzesuniecieZJednaPozycja(magazynWydaniaId, magazynPrzyjeciaId, produktId, 10);
            await new PrzesuniecieApiCaller(client).Przesun(przesuniecieModel);

            //Act
            var przesuniecieModel2 = DokumentObjectMother.GetPrzesuniecieZJednaPozycja(magazynWydaniaId, magazynPrzyjeciaId, produktId, 1);

            var serializerSettings = GetNodaTimeSerializerSettings();
            var content = new StringContent(JsonConvert.SerializeObject(przesuniecieModel2, serializerSettings), Encoding.UTF8, "application/json");
            var result = await client.PostAsync("Przesuniecie/Przesun", content).ConfigureAwait(false);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            var contentString = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
            Assert.That(contentString, Is.EqualTo("Niewystarczający stan magazynowy"));
        }

        [Test]
        public async Task CheckStanAktualnyAfterPrzesuniecie()
        {
            // Arrange
            var client = _factory.CreateClient();
            var magazynApiCaller = new MagazynApiCaller(client);

            var tokens = await Authenticate(client).ConfigureAwait(false);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.Token);

            var magazynWydaniaId = await magazynApiCaller.DodajMagazyn(MagazynObjectMother.GetMagazyn());
            var magazynPrzyjeciaId = await magazynApiCaller.DodajMagazyn(MagazynObjectMother.GetMagazyn());
            var produktId = await new ProduktApiCaller(client).DodajProdukt(ProduktObjectMother.GetProdukt(magazynWydaniaId));

            var dokumentPrzyjecia = DokumentObjectMother.GetDokumentPrzyjeciaZJednaPozycja(magazynWydaniaId, produktId, 10);
            await new PrzyjecieApiCaller(client).Przyjmij(dokumentPrzyjecia);

            var przesuniecieModel = DokumentObjectMother.GetPrzesuniecieZJednaPozycja(magazynWydaniaId, magazynPrzyjeciaId, produktId, 7);

            var stanAktualnyApiCaller = new StanAktualnyApiCaller(client);

            await new PrzesuniecieApiCaller(client).Przesun(przesuniecieModel);

            var stanyWydane = await stanAktualnyApiCaller.GetStanAktualny(magazynWydaniaId);
            Assert.That(stanyWydane, Has.Count.EqualTo(1));
            Assert.That(stanyWydane.First().Ilosc, Is.EqualTo(3));

            var stanyPrzyjete = await stanAktualnyApiCaller.GetStanAktualny(magazynPrzyjeciaId);
            Assert.That(stanyPrzyjete, Has.Count.EqualTo(1));
            Assert.That(stanyPrzyjete.First().Ilosc, Is.EqualTo(7));
        }
    }
}