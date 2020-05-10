using Bogus;
using MagazynManager.Infrastructure.InputModel.Slowniki;

namespace MagazynManager.Tests.ObjectMothers
{
    public static class MagazynObjectMother
    {
        public static MagazynCreateModel GetMagazyn()
        {
            return new Faker<MagazynCreateModel>()
                .RuleFor(x => x.Skrot, x => x.Commerce.ProductName())
                .RuleFor(x => x.Nazwa, x => x.Commerce.ProductName())
                .Generate();
        }
    }
}