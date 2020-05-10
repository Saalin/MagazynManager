using MagazynManager.Tests.IntegrationTests.ApiCallers;
using MagazynManager.Tests.ObjectMothers;
using NUnit.Framework;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MagazynManager.Tests.IntegrationTests
{
    [TestFixture]
    public class JednostkaMiaryTests : AuthorizedTest
    {
        [Test]
        public async Task Add_JednostkaMiary_And_Check_Count()
        {
            // Arrange
            var client = _factory.CreateClient();
            var apiCaller = new JednostkaMiaryApiCaller(client);

            var tokens = await Authenticate(client).ConfigureAwait(false);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.Token);

            await apiCaller.AddJednostkaMiary(JednostkaMiaryObjectMother.GetJednostkaMiary());

            var jednostkiMiary = await apiCaller.GetJednostkiMiary();

            Assert.That(jednostkiMiary, Is.Not.Null);
            Assert.That(jednostkiMiary, Has.Count.EqualTo(1));
        }
    }
}