using FluentValidation;

namespace MagazynManager.Infrastructure.InputModel.Ewidencja.Validators
{
    public class PrzyjeciePozycjaDokumentuCreateModelValidator : AbstractValidator<PrzyjeciePozycjaDokumentuCreateModel>
    {
        public PrzyjeciePozycjaDokumentuCreateModelValidator()
        {
            RuleFor(x => x.CenaNetto).GreaterThan(0);
            RuleFor(x => x.Ilosc).GreaterThan(0);
            RuleFor(x => x.StawkaVat).IsInEnum();
            RuleFor(x => x.ProduktId).NotEmpty();
        }
    }
}