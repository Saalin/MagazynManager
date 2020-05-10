using FluentValidation;

namespace MagazynManager.Infrastructure.InputModel.Rezerwacje.Validators
{
    public class RezerwacjaCreateModelValidator : AbstractValidator<RezerwacjaCreateModel>
    {
        public RezerwacjaCreateModelValidator()
        {
            RuleFor(x => x.DataRezerwacji).NotEmpty();
            RuleFor(x => x.DataWaznosci).NotEmpty();
            RuleFor(x => x.Opis).NotNull();
            RuleFor(m => new { m.DataRezerwacji, m.DataWaznosci }).Must(x => x.DataWaznosci > x.DataRezerwacji);
        }
    }
}