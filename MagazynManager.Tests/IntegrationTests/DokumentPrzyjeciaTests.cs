using MagazynManager.Domain.Entities.Slowniki;
using MagazynManager.Infrastructure.InputModel.Ewidencja;
using MagazynManager.Tests.IntegrationTests.ApiCallers;
using MagazynManager.Tests.ObjectMothers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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
            Assert.That(response, Is.Empty);
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

            var przyjecieModel = new PrzyjecieCreateModel
            {
                MagazynId = magazynId,
                Data = DateTime.Now,
                Pozycje = new List<PrzyjeciePozycjaDokumentuCreateModel>
                {
                    new PrzyjeciePozycjaDokumentuCreateModel
                    {
                        ProduktId = produktId,
                        CenaNetto = 1M,
                        Ilosc = 10,
                        StawkaVat = StawkaVat.DwadziesciaTrzyProcent
                    }
                }
            };

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

            var przyjecieModel = new PrzyjecieCreateModel
            {
                MagazynId = magazynId,
                Data = DateTime.Now,
                Pozycje = new List<PrzyjeciePozycjaDokumentuCreateModel>
                {
                    new PrzyjeciePozycjaDokumentuCreateModel
                    {
                        ProduktId = produktId,
                        CenaNetto = 1M,
                        Ilosc = 10,
                        StawkaVat = StawkaVat.DwadziesciaTrzyProcent
                    }
                }
            };

            await apiCaller.Przyjmij(przyjecieModel);

            var stany = await new StanAktualnyApiCaller(client).GetStanAktualny(magazynId);

            Assert.That(stany, Has.Count.EqualTo(1));
        }
    }
}