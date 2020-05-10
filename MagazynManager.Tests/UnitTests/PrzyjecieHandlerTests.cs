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
using System.Threading;
using System.Threading.Tasks;

namespace MagazynManager.Tests.UnitTests
{
    [Category("UnitTest")]
    [TestFixture]
    public class PrzyjecieHandlerTests
    {
        private Guid MagazynId = Guid.NewGuid();
        private Guid PrzedsiebiorstwoId = Guid.NewGuid();

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

            Assert.That(stanAktualnyList, Has.Count.EqualTo(1));
        }
    }
}