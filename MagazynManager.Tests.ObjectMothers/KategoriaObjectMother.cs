using Bogus;
using MagazynManager.Infrastructure.InputModel.Slowniki;

namespace MagazynManager.Tests.ObjectMothers
{
    public static class KategoriaObjectMother
    {
        public static KategoriaCreateModel GetKategoria()
        {
            return new Faker<KategoriaCreateModel>()
                .RuleFor(x => x.Name, f => f.Name.Random.AlphaNumeric(10))
                .Generate();
        }
    }
}