using FluentValidation;

namespace MagazynManager.Infrastructure.InputModel.Rezerwacje.Validators
{
    public class PozycjaRezerwacjiCreateModelValidator : AbstractValidator<PozycjaRezerwacjiCreateModel>
    {
        public PozycjaRezerwacjiCreateModelValidator()
        {
            RuleFor(x => x.ProduktId).NotEmpty();
            RuleFor(x => x.Ilosc).GreaterThan(0);
        }
    }
}