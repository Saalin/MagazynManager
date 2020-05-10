using FluentValidation;

namespace MagazynManager.Infrastructure.InputModel.Ewidencja.Validators
{
    public class PozycjaWydaniaModelValidator : AbstractValidator<PozycjaWydaniaModel>
    {
        public PozycjaWydaniaModelValidator()
        {
            RuleFor(x => x.ProduktId).NotEmpty();
            RuleFor(x => x.Ilosc).GreaterThan(0);
        }
    }
}