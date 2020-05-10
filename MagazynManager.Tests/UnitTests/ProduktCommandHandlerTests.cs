using MagazynManager.Application.CommandHandlers.Slowniki;
using MagazynManager.Application.Commands.Slowniki;
using MagazynManager.Application.Queries.Slowniki;
using MagazynManager.Application.QueryHandlers.Slowniki;
using MagazynManager.Domain.Entities.Produkty;
using MagazynManager.Tests.UnitTests.Fakes;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MagazynManager.Tests.UnitTests
{
    [Category("UnitTest")]
    [TestFixture]
    public class ProduktCommandHandlerTests
    {
        private Guid MagazynId = Guid.NewGuid();
        private Guid PrzedsiebiorstwoId = Guid.NewGuid();

        private IProduktRepository _produktRepository;
        private IJednostkaMiaryRepository _jednostkaMiaryRepository;
        private IKategoriaRepository _kategoriaRepository;

        [SetUp]
        public void Setup()
        {
            _produktRepository = new InMemoryProduktRepository();
            _jednostkaMiaryRepository = new InMemoryJednostkaMiaryRepository();
            _kategoriaRepository = new InMemoryKategoriaRepository();
        }

        [Test]
        public async Task TestAddProdukt()
        {
            var commandHandler = new ProduktCommandHandler(_produktRepository, _kategoriaRepository, _jednostkaMiaryRepository);
            await commandHandler.Handle(new ProduktCreateCommand("Broku�", "Broku�", "szt", "Warzywa", MagazynId, PrzedsiebiorstwoId), new CancellationToken());

            var queryHandler = new ProduktListQueryHandler(_produktRepository);
            var produktList = await queryHandler.Handle(new ProduktListQuery(PrzedsiebiorstwoId), new CancellationToken());

            Assert.That(produktList, Has.Count.EqualTo(1));
        }

        [Test]
        public async Task DeleteProdukt()
        {
            var commandHandler = new ProduktCommandHandler(_produktRepository, _kategoriaRepository, _jednostkaMiaryRepository);
            var produktId = await commandHandler.Handle(new ProduktCreateCommand("Broku�", "Broku�", "szt", "Warzywa", MagazynId, PrzedsiebiorstwoId), new CancellationToken());

            var queryHandler = new ProduktListQueryHandler(_produktRepository);
            var produktList = await queryHandler.Handle(new ProduktListQuery(PrzedsiebiorstwoId), new CancellationToken());

            Assert.That(produktList, Has.Count.EqualTo(1));

            await commandHandler.Handle(new ProduktDeleteCommand(produktId, PrzedsiebiorstwoId), new CancellationToken());

            var produktListAfterDelete = await queryHandler.Handle(new ProduktListQuery(PrzedsiebiorstwoId), new CancellationToken());

            Assert.That(produktListAfterDelete, Is.Empty);
        }
    }
}