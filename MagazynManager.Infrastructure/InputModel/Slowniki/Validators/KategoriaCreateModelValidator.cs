using FluentValidation;

namespace MagazynManager.Infrastructure.InputModel.Slowniki.Validators
{
    public class KategoriaCreateModelValidator : AbstractValidator<KategoriaCreateModel>
    {
        public KategoriaCreateModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull();
        }
    }
}