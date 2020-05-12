using MagazynManager.Tests.IntegrationTests.ApiCallers;
using MagazynManager.Tests.ObjectMothers;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MagazynManager.Tests.IntegrationTests
{
    [TestFixture]
    public class KategoriaTests : AuthorizedTest
    {
        [Test]
        public async Task Get_CategoriesListAfterAuthorization()
        {
            // Arrange
            var client = _factory.CreateClient();

            var tokens = await Authenticate(client).ConfigureAwait(false);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.Token);

            // Act
            var response = await new KategoriaApiCaller(client).GetKategorieList();

            Assert.That(response, Is.Not.Null);
        }

        [Test]
        public async Task Add_Kategoria_And_Check_Count()
        {
            // Arrange
            var client = _factory.CreateClient();
            var apiCaller = new KategoriaApiCaller(client);

            var tokens = await Authenticate(client).ConfigureAwait(false);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.Token);

            var categoriesCountPrzedDodaniem = (await apiCaller.GetKategorieList()).Count;

            await apiCaller.DodajKategorie(KategoriaObjectMother.GetKategoria());

            var categories = await apiCaller.GetKategorieList();

            Assert.That(categories, Is.Not.Null);
            Assert.That(categories, Has.Count.EqualTo(categoriesCountPrzedDodaniem + 1));
        }

        [Test]
        public async Task DeleteKategoria()
        {
            // Arrange
            var client = _factory.CreateClient();
            var apiCaller = new KategoriaApiCaller(client);

            var tokens = await Authenticate(client).ConfigureAwait(false);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.Token);

            var id = await apiCaller.DodajKategorie(KategoriaObjectMother.GetKategoria());

            var categoriesBeforeDeletion = await apiCaller.GetKategorieList();

            var deleteResponse = await apiCaller.UsunKategorie(id);
            Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

            var categoriesAfterDeletionCount = (await apiCaller.GetKategorieList()).Count;
            Assert.That(categoriesAfterDeletionCount, Is.EqualTo(categoriesBeforeDeletion.Count - 1));
        }
    }
}