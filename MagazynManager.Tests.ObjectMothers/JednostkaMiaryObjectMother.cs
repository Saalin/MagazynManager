using Bogus;
using MagazynManager.Infrastructure.InputModel.Slowniki;

namespace MagazynManager.Tests.ObjectMothers
{
    public static class JednostkaMiaryObjectMother
    {
        public static JednostkaMiaryCreateModel GetJednostkaMiary()
        {
            return new Faker<JednostkaMiaryCreateModel>()
                .RuleFor(x => x.Nazwa, f => f.Name.Random.AlphaNumeric(10))
                .Generate();
        }
    }
}