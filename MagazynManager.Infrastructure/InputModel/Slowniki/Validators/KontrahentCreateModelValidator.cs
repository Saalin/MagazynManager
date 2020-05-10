using FluentValidation;

namespace MagazynManager.Infrastructure.InputModel.Slowniki.Validators
{
    public class KontrahentCreateModelValidator : AbstractValidator<KontrahentCreateModel>
    {
        public KontrahentCreateModelValidator()
        {
            RuleFor(x => x.Nazwa).NotEmpty().NotNull();
            RuleFor(x => x.Skrot).NotEmpty().NotNull();
            RuleFor(x => x.TypKontrahenta).IsInEnum();
            RuleFor(x => x.DaneAdresowe).NotNull();
            RuleFor(x => x.Nip).NotEmpty();
        }
    }
}