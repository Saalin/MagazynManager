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
    public class JednostkiMiaryCommandHandlerTests : UnitTest
    {
        private ISlownikRepository<JednostkaMiary> _jednostkaMiaryRepository;

        [SetUp]
        public void Setup()
        {
            _jednostkaMiaryRepository = new InMemoryJednostkaMiaryRepository();
        }

        [Test]
        public async Task TestAddJednostkaMiary()
        {
            var jednostkaMiary = ObjectMothers.JednostkaMiaryObjectMother.GetJednostkaMiary();

            var commandHandler = new JednostkaMiaryCommandHandler(_jednostkaMiaryRepository);
            await commandHandler.Handle(new JednostkaMiaryCreateCommand(PrzedsiebiorstwoId, jednostkaMiary.Nazwa), new CancellationToken());

            var queryHandler = new JednostkaMiaryListQueryHandler(_jednostkaMiaryRepository);
            var jednostkiMiaryList = await queryHandler.Handle(new JednostkaMiaryListQuery(PrzedsiebiorstwoId), new CancellationToken());

            Assert.That(jednostkiMiaryList, Has.Count.EqualTo(1));
            Assert.That(jednostkiMiaryList.First().Name, Is.EqualTo(jednostkaMiary.Nazwa));
        }

        [Test]
        public async Task DeleteJednostka()
        {
            var jednostkaMiary = ObjectMothers.JednostkaMiaryObjectMother.GetJednostkaMiary();

            var commandHandler = new JednostkaMiaryCommandHandler(_jednostkaMiaryRepository);
            var jednostkMiaryId = await commandHandler.Handle(new JednostkaMiaryCreateCommand(PrzedsiebiorstwoId, jednostkaMiary.Nazwa), new CancellationToken());

            var queryHandler = new JednostkaMiaryListQueryHandler(_jednostkaMiaryRepository);
            var jednostkiMiaryList = await queryHandler.Handle(new JednostkaMiaryListQuery(PrzedsiebiorstwoId), new CancellationToken());

            Assert.That(jednostkiMiaryList, Has.Count.EqualTo(1));

            await commandHandler.Handle(new JednostkaMiaryDeleteCommand(jednostkMiaryId, PrzedsiebiorstwoId), new CancellationToken());

            var jednostkiMiaryAfterDelete = await queryHandler.Handle(new JednostkaMiaryListQuery(PrzedsiebiorstwoId), new CancellationToken());

            Assert.That(jednostkiMiaryAfterDelete, Is.Empty);
        }
    }
}