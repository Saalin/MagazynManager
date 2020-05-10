using Bogus;
using MagazynManager.Domain.Entities.Kontrahent;
using MagazynManager.Infrastructure.InputModel.Slowniki;

namespace MagazynManager.Tests.ObjectMothers
{
    public static class KontrahentObjectMother
    {
        public static KontrahentCreateModel GetKontrahent()
        {
            var daneAdresowe = new Faker<DaneAdresoweCreateModel>()
                .RuleFor(x => x.Ulica, x => x.Address.StreetAddress())
                .RuleFor(x => x.Miejscowosc, x => x.Address.City())
                .RuleFor(x => x.KodPocztowy, _ => "00-000")
                .Generate();

            var kontrahent = new Faker<KontrahentCreateModel>()
                .StrictMode(false)
                .RuleFor(x => x.Skrot, x => x.Commerce.ProductName())
                .RuleFor(x => x.Nazwa, x => x.Commerce.ProductName())
                .RuleFor(x => x.Nip, x => x.Random.String(10))
                .RuleFor(x => x.TypKontrahenta, x => x.PickRandom<TypKontrahenta>())
                .Generate();

            kontrahent.DaneAdresowe = daneAdresowe;

            return kontrahent;
        }
    }
}