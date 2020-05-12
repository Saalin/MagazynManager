using MagazynManager.Application;
using MagazynManager.Application.CommandHandlers.Ewidencja;
using MagazynManager.Application.CommandHandlers.Slowniki;
using MagazynManager.Application.Commands.Ewidencja;
using MagazynManager.Application.Commands.Slowniki;
using MagazynManager.Application.Queries.Ewidencja;
using MagazynManager.Application.QueryHandlers.Ewidencja;
using MagazynManager.Domain.DomainServices;
using MagazynManager.Domain.Entities.Dokumenty;
using MagazynManager.Domain.Entities.Produkty;
using MagazynManager.Domain.Entities.Slowniki;
using MagazynManager.Infrastructure.InputModel.Ewidencja;
using MagazynManager.Tests.UnitTests.Fakes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MagazynManager.Tests.UnitTests.Ewidencja
{
    [Category("UnitTest")]
    [TestFixture]
    public class EwidencjaTests : UnitTest
    {
        private Guid MagazynId = Guid.NewGuid();

        private IProduktRepository _produktRepository;
        private IJednostkaMiaryRepository _jednostkaMiaryRepository;
        private IKategoriaRepository _kategoriaRepository;
        private IDokumentRepository _dokumentRepository;

        [SetUp]
        public void Setup()
        {
            _produktRepository = new InMemoryProduktRepository();
            _jednostkaMiaryRepository = new InMemoryJednostkaMiaryRepository();
            _kategoriaRepository = new InMemoryKategoriaRepository();
            _dokumentRepository = new InMemoryDokumentRepository();
        }

        [Test]
        public async Task ZmienionyStanAktualnyPoPrzyjeciu()
        {
            var produktAddCommandHandler = new ProduktCommandHandler(_produktRepository, _kategoriaRepository, _jednostkaMiaryRepository);
            var produktId = await produktAddCommandHandler.Handle(new ProduktCreateCommand("Broku�", "Broku�", "szt", "Warzywa", MagazynId, PrzedsiebiorstwoId), new CancellationToken());

            var commandHandler = new PrzyjmijCommandHandler(_dokumentRepository);
            await commandHandler.Handle(new PrzyjmijCommand(PrzedsiebiorstwoId, new PrzyjecieCreateModel
            {
                MagazynId = MagazynId,
                KontrahentId = null,
                Data = DateTime.Now,
                Pozycje = new List<PrzyjeciePozycjaDokumentuCreateModel>
                {
                    new PrzyjeciePozycjaDokumentuCreateModel
                    {
                        Ilosc = 42,
                        CenaNetto = 1,
                        ProduktId = produktId,
                        StawkaVat = StawkaVat.PiecProcent
                    }
                }
            }), new CancellationToken());

            var stanQueryHandler = new StanAktualnyMagazynuQueryHandler(new StanAktualnyService(_dokumentRepository), _produktRepository);
            var stanAktualnyList = await stanQueryHandler.Handle(new StanAktualnyMagazynuQuery(MagazynId, PrzedsiebiorstwoId), new CancellationToken());

            var dokumentyPrzyjeciaQueryHandler = new DokumentPrzyjeciaListQueryHandler(_dokumentRepository);
            var dokumentyPrzyjecia = await dokumentyPrzyjeciaQueryHandler.Handle(new DokumentPrzyjeciaListQuery(PrzedsiebiorstwoId), new CancellationToken());

            Assert.That(dokumentyPrzyjecia, Has.Count.EqualTo(1));
            Assert.That(stanAktualnyList, Has.Count.EqualTo(1));
        }

        [Test]
        public async Task WydanieTowaruPoPrzyjeciu()
        {
            await ZmienionyStanAktualnyPoPrzyjeciu();

            var stanQueryHandler = new StanAktualnyMagazynuQueryHandler(new StanAktualnyService(_dokumentRepository), _produktRepository);
            var stanAktualnyList = await stanQueryHandler.Handle(new StanAktualnyMagazynuQuery(MagazynId, PrzedsiebiorstwoId), new CancellationToken());

            Assert.That(stanAktualnyList, Has.Count.GreaterThan(0));

            var stanProduktuPrzedWydaniem = stanAktualnyList.First();

            var commandHandler = new WydajCommandHandler(_dokumentRepository, new StanAktualnyService(_dokumentRepository));
            await commandHandler.Handle(new WydajCommand(PrzedsiebiorstwoId, new WydanieCreateModel
            {
                MagazynId = MagazynId,
                KontrahentId = null,
                Data = DateTime.Now,
                Pozycje = new List<PozycjaWydaniaModel>
                {
                    new PozycjaWydaniaModel
                    {
                        Ilosc = 10,
                        ProduktId = stanProduktuPrzedWydaniem.ProduktId
                    }
                }
            }), new CancellationToken());

            var stanAktualnyAfterWydanie = await stanQueryHandler.Handle(new StanAktualnyMagazynuQuery(MagazynId, PrzedsiebiorstwoId), new CancellationToken());

            var stanyPoWydaniu = stanAktualnyAfterWydanie.Single(x => x.ProduktId == stanProduktuPrzedWydaniem.ProduktId);

            Assert.That(stanyPoWydaniu.Ilosc, Is.EqualTo(stanProduktuPrzedWydaniem.Ilosc - 10));
        }

        [Test]
        public async Task ProbaWydaniaPonadStan()
        {
            await ZmienionyStanAktualnyPoPrzyjeciu();

            var stanQueryHandler = new StanAktualnyMagazynuQueryHandler(new StanAktualnyService(_dokumentRepository), _produktRepository);
            var stanAktualnyList = await stanQueryHandler.Handle(new StanAktualnyMagazynuQuery(MagazynId, PrzedsiebiorstwoId), new CancellationToken());

            Assert.That(stanAktualnyList, Has.Count.GreaterThan(0));

            var stanProduktuPrzedWydaniem = stanAktualnyList.First();

            var commandHandler = new WydajCommandHandler(_dokumentRepository, new StanAktualnyService(_dokumentRepository));

            Assert.ThrowsAsync<BussinessException>(async () =>
            {
                await commandHandler.Handle(new WydajCommand(PrzedsiebiorstwoId, new WydanieCreateModel
                {
                    MagazynId = MagazynId,
                    KontrahentId = null,
                    Data = DateTime.Now,
                    Pozycje = new List<PozycjaWydaniaModel>
                {
                    new PozycjaWydaniaModel
                    {
                        Ilosc = stanProduktuPrzedWydaniem.Ilosc + 10,
                        ProduktId = stanProduktuPrzedWydaniem.ProduktId
                    }
                }
                }), new CancellationToken());
            });
        }
    }
}