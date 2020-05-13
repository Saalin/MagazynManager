using MagazynManager.Tests.IntegrationTests.ApiCallers;
using MagazynManager.Tests.ObjectMothers;
using NUnit.Framework;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MagazynManager.Tests.IntegrationTests
{
    [TestFixture]
    public class RezerwacjeTests : AuthorizedTest
    {
        [Test]
        public async Task Rezerwuj_Sprawdz_Liste()
        {
            // Arrange
            var client = _factory.CreateClient();
            var apiCaller = new RezerwacjeApiCaller(client);

            var tokens = await Authenticate(client).ConfigureAwait(false);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.Token);

            var countPrzedRezerwacja = (await apiCaller.GetList()).Count;

            var magazynId = await new MagazynApiCaller(client).DodajMagazyn(MagazynObjectMother.GetMagazyn());
            var produktId = await new ProduktApiCaller(client).DodajProdukt(ProduktObjectMother.GetProdukt(magazynId));

            var rezerwacja = RezerwacjaObjectMother.GetPoprawanaRezerwacja(produktId);
            await apiCaller.Rezerwuj(rezerwacja);

            var listaRezerwacji = await apiCaller.GetList();

            Assert.That(listaRezerwacji, Has.Count.EqualTo(countPrzedRezerwacja + 1));
        }

        [Test]
        public async Task UsunRezerwacjeSprawdzListe()
        {
            // Arrange
            var client = _factory.CreateClient();
            var apiCaller = new RezerwacjeApiCaller(client);

            var tokens = await Authenticate(client).ConfigureAwait(false);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.Token);

            var magazynId = await new MagazynApiCaller(client).DodajMagazyn(MagazynObjectMother.GetMagazyn());
            var produktId = await new ProduktApiCaller(client).DodajProdukt(ProduktObjectMother.GetProdukt(magazynId));

            var rezerwacja = RezerwacjaObjectMother.GetPoprawanaRezerwacja(produktId);
            await apiCaller.Rezerwuj(rezerwacja);

            var listaRezerwacjiPrzedUsunieciem = await apiCaller.GetList();

            await apiCaller.Anuluj(listaRezerwacjiPrzedUsunieciem.First().Id);

            var listaRezerwacjiPoUsunieciu = await apiCaller.GetList();

            Assert.That(listaRezerwacjiPoUsunieciu, Has.Count.EqualTo(listaRezerwacjiPrzedUsunieciem.Count - 1));
        }

        [Test]
        public async Task UsunPrzedawnioneRezerwacje()
        {
            // Arrange
            var client = _factory.CreateClient();
            var apiCaller = new RezerwacjeApiCaller(client);

            var tokens = await Authenticate(client).ConfigureAwait(false);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.Token);

            var magazynId = await new MagazynApiCaller(client).DodajMagazyn(MagazynObjectMother.GetMagazyn());
            var produktId = await new ProduktApiCaller(client).DodajProdukt(ProduktObjectMother.GetProdukt(magazynId));

            var rezerwacja = RezerwacjaObjectMother.GetPrzedawnionaRezerwacja(produktId);

            await apiCaller.Rezerwuj(rezerwacja);

            var listaRezerwacji = await apiCaller.GetList();
            var liczbaPrzedawnionych = listaRezerwacji.Count(x => x.DataWaznosci < DateTime.Now);

            await apiCaller.UsunPrzedawnione();

            var listaRezerwacjiPoUsunieciu = await apiCaller.GetList();

            Assert.That(listaRezerwacjiPoUsunieciu, Has.Count.EqualTo(listaRezerwacji.Count - liczbaPrzedawnionych));
        }
    }
}