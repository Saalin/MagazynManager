using FluentValidation;

namespace MagazynManager.Infrastructure.InputModel.Slowniki.Validators
{
    public class ProduktCreateModelValidator : AbstractValidator<ProduktCreateModel>
    {
        public ProduktCreateModelValidator()
        {
            RuleFor(x => x.ShortName).NotEmpty().NotNull();
            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.JednostkaMiary).NotEmpty().NotNull();
            RuleFor(x => x.Kategoria).NotEmpty().NotNull();
            RuleFor(x => x.MagazynId).NotEmpty();
        }
    }
}