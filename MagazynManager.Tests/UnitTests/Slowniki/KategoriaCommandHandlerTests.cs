using MagazynManager.Application.CommandHandlers.Slowniki;
using MagazynManager.Application.Commands.Slowniki;
using MagazynManager.Application.Queries.Slowniki;
using MagazynManager.Application.QueryHandlers.Slowniki;
using MagazynManager.Domain.Entities;
using MagazynManager.Domain.Entities.Produkty;
using MagazynManager.Tests.UnitTests.Fakes;
using NUnit.Framework;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MagazynManager.Tests.UnitTests.Slowniki
{
    [Category("UnitTest")]
    [TestFixture]
    public class KategoriaCommandHandlerTests : UnitTest
    {
        private ISlownikRepository<Kategoria> _kategoriaRepository;

        [SetUp]
        public void Setup()
        {
            _kategoriaRepository = new InMemoryKategoriaRepository();
        }

        [Test]
        public async Task TestAddJednostkaMiary()
        {
            var kategoria = ObjectMothers.KategoriaObjectMother.GetKategoria();

            var commandHandler = new KategoriaCommandHandler(_kategoriaRepository);
            await commandHandler.Handle(new KategoriaCreateCommand(kategoria, PrzedsiebiorstwoId), new CancellationToken());

            var queryHandler = new KategoriaListQueryHandler(_kategoriaRepository);
            var kategorieList = await queryHandler.Handle(new KategoriaListQuery(PrzedsiebiorstwoId), new CancellationToken());

            Assert.That(kategorieList, Has.Count.EqualTo(1));
            Assert.That(kategorieList.First().Name, Is.EqualTo(kategoria.Name));
        }

        [Test]
        public async Task DeleteKategoria()
        {
            var kategoria = ObjectMothers.KategoriaObjectMother.GetKategoria();

            var commandHandler = new KategoriaCommandHandler(_kategoriaRepository);
            var kategoriaId = await commandHandler.Handle(new KategoriaCreateCommand(kategoria, PrzedsiebiorstwoId), new CancellationToken());

            var queryHandler = new KategoriaListQueryHandler(_kategoriaRepository);
            var jednostkiMiaryList = await queryHandler.Handle(new KategoriaListQuery(PrzedsiebiorstwoId), new CancellationToken());

            Assert.That(jednostkiMiaryList, Has.Count.EqualTo(1));

            await commandHandler.Handle(new KategoriaDeleteCommand(kategoriaId), new CancellationToken());

            var kategorieAfterDelete = await queryHandler.Handle(new KategoriaListQuery(PrzedsiebiorstwoId), new CancellationToken());

            Assert.That(kategorieAfterDelete, Is.Empty);
        }
    }
}