using FluentValidation;

namespace MagazynManager.Infrastructure.InputModel.Slowniki.Validators
{
    public class DaneAdresoweModelValidator : AbstractValidator<DaneAdresoweCreateModel>
    {
        public DaneAdresoweModelValidator()
        {
            RuleFor(x => x.Ulica).NotEmpty().NotNull();
            RuleFor(x => x.Miejscowosc).NotEmpty().NotNull();
            RuleFor(x => x.KodPocztowy).Matches("[0-9]{2}-[0-9]{3}").NotEmpty();
        }
    }
}