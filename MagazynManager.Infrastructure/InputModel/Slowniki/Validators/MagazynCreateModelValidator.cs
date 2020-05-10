using FluentValidation;

namespace MagazynManager.Infrastructure.InputModel.Slowniki.Validators
{
    public class MagazynCreateModelValidator : AbstractValidator<MagazynCreateModel>
    {
        public MagazynCreateModelValidator()
        {
            RuleFor(x => x.Nazwa).NotEmpty().NotNull();
            RuleFor(x => x.Skrot).NotEmpty().NotNull();
        }
    }
}