using MagazynManager.Application;
using MagazynManager.Application.CommandHandlers.Ewidencja;
using MagazynManager.Application.CommandHandlers.Slowniki;
using MagazynManager.Application.Commands.Ewidencja;
using MagazynManager.Application.Commands.Slowniki;
using MagazynManager.Application.Queries.Ewidencja;
using MagazynManager.Application.QueryHandlers.Ewidencja;
using MagazynManager.Domain.DomainServices;
using MagazynManager.Domain.Entities;
using MagazynManager.Domain.Entities.Dokumenty;
using MagazynManager.Domain.Entities.Kontrahent;
using MagazynManager.Domain.Entities.Produkty;
using MagazynManager.Tests.ObjectMothers;
using MagazynManager.Tests.UnitTests.Fakes;
using NUnit.Framework;
using System;
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
        private ISlownikRepository<Kategoria> _kategoriaRepository;
        private IDokumentRepository _dokumentRepository;

        private Guid KontrahentId { get; set; }

        [SetUp]
        public void Setup()
        {
            _produktRepository = new InMemoryProduktRepository();
            _jednostkaMiaryRepository = new InMemoryJednostkaMiaryRepository();
            _kategoriaRepository = new InMemoryKategoriaRepository();
            _dokumentRepository = new InMemoryDokumentRepository();

            var kontrahentRepository = new InMemoryKontrahentRepository();
            KontrahentId = kontrahentRepository.Save(new Kontrahent
            {
                Id = Guid.NewGuid(),
                DaneAdresowe = new DaneAdresowe
                {
                    KodPocztowy = "12-345",
                    Miejscowosc = "Warszawa",
                    Ulica = "Puławska 10"
                },
                Nazwa = "Abc",
                Nip = "123-134-23-23",
                PrzedsiebiorstwoId = PrzedsiebiorstwoId,
                Skrot = "Abc",
                TypKontrahenta = TypKontrahenta.Firma
            }).Result;
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task ZmienionyStanAktualnyPoPrzyjeciu(bool kontrahent)
        {
            var produktAddCommandHandler = new ProduktCommandHandler(_produktRepository, _kategoriaRepository, _jednostkaMiaryRepository);
            var produkt = ProduktObjectMother.GetProdukt(MagazynId);
            var produktId = await produktAddCommandHandler.Handle(new ProduktCreateCommand(produkt.Name, produkt.ShortName, produkt.JednostkaMiary, produkt.Kategoria, MagazynId, PrzedsiebiorstwoId), new CancellationToken());

            var commandHandler = new PrzyjmijCommandHandler(_dokumentRepository);
            var dokumentPrzyjecia = DokumentObjectMother.GetDokumentPrzyjeciaZJednaPozycja(MagazynId, produktId, 42, kontrahent ? KontrahentId : (Guid?)null);
            await commandHandler.Handle(new PrzyjmijCommand(PrzedsiebiorstwoId, dokumentPrzyjecia), new CancellationToken());

            var stanQueryHandler = new StanAktualnyMagazynuQueryHandler(new StanAktualnyService(_dokumentRepository), _produktRepository);
            var stanAktualnyList = await stanQueryHandler.Handle(new StanAktualnyMagazynuQuery(MagazynId, PrzedsiebiorstwoId), new CancellationToken());

            var dokumentyPrzyjeciaQueryHandler = new DokumentPrzyjeciaListQueryHandler(_dokumentRepository);
            var dokumentyPrzyjecia = await dokumentyPrzyjeciaQueryHandler.Handle(new DokumentPrzyjeciaListQuery(PrzedsiebiorstwoId), new CancellationToken());

            Assert.That(dokumentyPrzyjecia, Has.Count.EqualTo(1));
            Assert.That(stanAktualnyList, Has.Count.EqualTo(1));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task WydanieTowaruPoPrzyjeciu(bool kontrahent)
        {
            await ZmienionyStanAktualnyPoPrzyjeciu(kontrahent);

            var stanQueryHandler = new StanAktualnyMagazynuQueryHandler(new StanAktualnyService(_dokumentRepository), _produktRepository);
            var stanAktualnyList = await stanQueryHandler.Handle(new StanAktualnyMagazynuQuery(MagazynId, PrzedsiebiorstwoId), new CancellationToken());

            Assert.That(stanAktualnyList, Has.Count.GreaterThan(0));

            var stanProduktuPrzedWydaniem = stanAktualnyList.First();

            var commandHandler = new WydajCommandHandler(_dokumentRepository, new StanAktualnyService(_dokumentRepository));
            var dokumentWydania = DokumentObjectMother.GetDokumentWydaniaZJednaPozycja(MagazynId, stanProduktuPrzedWydaniem.ProduktId, 10, kontrahent ? KontrahentId : (Guid?)null);
            await commandHandler.Handle(new WydajCommand(PrzedsiebiorstwoId, dokumentWydania), new CancellationToken());

            var stanAktualnyAfterWydanie = await stanQueryHandler.Handle(new StanAktualnyMagazynuQuery(MagazynId, PrzedsiebiorstwoId), new CancellationToken());

            var stanyPoWydaniu = stanAktualnyAfterWydanie.Single(x => x.ProduktId == stanProduktuPrzedWydaniem.ProduktId);

            Assert.That(stanyPoWydaniu.Ilosc, Is.EqualTo(stanProduktuPrzedWydaniem.Ilosc - 10));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task ProbaWydaniaPonadStan(bool kontrahent)
        {
            await ZmienionyStanAktualnyPoPrzyjeciu(kontrahent);

            var stanQueryHandler = new StanAktualnyMagazynuQueryHandler(new StanAktualnyService(_dokumentRepository), _produktRepository);
            var stanAktualnyList = await stanQueryHandler.Handle(new StanAktualnyMagazynuQuery(MagazynId, PrzedsiebiorstwoId), new CancellationToken());

            Assert.That(stanAktualnyList, Has.Count.GreaterThan(0));

            var stanProduktuPrzedWydaniem = stanAktualnyList.First();

            var commandHandler = new WydajCommandHandler(_dokumentRepository, new StanAktualnyService(_dokumentRepository));

            var dokumentWydajacyZaDuzo = DokumentObjectMother.GetDokumentWydaniaZJednaPozycja(MagazynId, stanProduktuPrzedWydaniem.ProduktId, stanProduktuPrzedWydaniem.Ilosc + 10, kontrahent ? KontrahentId : (Guid?)null);
            Assert.ThrowsAsync<BussinessException>(async () =>
            {
                await commandHandler.Handle(new WydajCommand(PrzedsiebiorstwoId, dokumentWydajacyZaDuzo), new CancellationToken());
            });
        }
    }
}