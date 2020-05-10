using Bogus;
using MagazynManager.Infrastructure.InputModel.Slowniki;
using System;

namespace MagazynManager.Tests.ObjectMothers
{
    public static class ProduktObjectMother
    {
        public static ProduktCreateModel GetProdukt(Guid magazynId)
        {
            return new Faker<ProduktCreateModel>()
                .RuleFor(x => x.ShortName, x => x.Commerce.ProductName())
                .RuleFor(x => x.Name, x => x.Commerce.ProductName())
                .RuleFor(x => x.JednostkaMiary, _ => "kg")
                .RuleFor(x => x.Kategoria, x => x.Commerce.ProductMaterial())
                .RuleFor(x => x.MagazynId, _ => magazynId)
                .Generate();
        }
    }
}