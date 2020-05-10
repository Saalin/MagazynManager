using FluentValidation;

namespace MagazynManager.Infrastructure.InputModel.Ewidencja.Validators
{
    public class WydanieCreateModelValidator : AbstractValidator<WydanieCreateModel>
    {
        public WydanieCreateModelValidator()
        {
            RuleFor(x => x.MagazynId).NotEmpty();
            RuleFor(x => x.Data).NotEmpty();
        }
    }
}