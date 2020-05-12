using MagazynManager.Infrastructure.InputModel.Rezerwacje;
using MagazynManager.Tests.IntegrationTests.ApiCallers;
using MagazynManager.Tests.ObjectMothers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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

            var rezerwacja = new RezerwacjaCreateModel
            {
                DataRezerwacji = DateTime.Now,
                DataWaznosci = DateTime.Now.AddDays(7),
                Opis = string.Empty,
                Pozycje = new List<PozycjaRezerwacjiCreateModel>
                {
                    new PozycjaRezerwacjiCreateModel
                    {
                        ProduktId = produktId,
                        Ilosc = 42
                    }
                }
            };

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

            await Rezerwuj_Sprawdz_Liste();
            var listaRezerwacji = await apiCaller.GetList();

            await apiCaller.Anuluj(listaRezerwacji.First().Id);

            var listaRezerwacjiPoUsunieciu = await apiCaller.GetList();

            Assert.That(listaRezerwacjiPoUsunieciu, Has.Count.EqualTo(listaRezerwacji.Count - 1));
        }
    }
}