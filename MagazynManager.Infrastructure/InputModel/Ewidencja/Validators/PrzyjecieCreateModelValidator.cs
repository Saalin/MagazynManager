using FluentValidation;

namespace MagazynManager.Infrastructure.InputModel.Ewidencja.Validators
{
    public class PrzyjecieCreateModelValidator : AbstractValidator<PrzyjecieCreateModel>
    {
        public PrzyjecieCreateModelValidator()
        {
            RuleFor(x => x.MagazynId).NotEmpty();
            RuleFor(x => x.Data).NotEmpty();
        }
    }
}