using FluentValidation;

namespace MagazynManager.Infrastructure.InputModel.Ewidencja.Validators
{
    public class PrzesuniecieCreateModelValidator : AbstractValidator<PrzesuniecieCreateModel>
    {
        public PrzesuniecieCreateModelValidator()
        {
            RuleFor(x => x.MagazynPrzyjmujacyId).NotEmpty();
            RuleFor(x => x.MagazynWydajacyId).NotEmpty();
            RuleFor(x => x.Data).NotEmpty();
        }
    }
}