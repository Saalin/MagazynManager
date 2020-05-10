using FluentValidation;

namespace MagazynManager.Infrastructure.InputModel.Slowniki.Validators
{
    public class JednostkaMiaryCreateModelValidator : AbstractValidator<JednostkaMiaryCreateModel>
    {
        public JednostkaMiaryCreateModelValidator()
        {
            RuleFor(x => x.Nazwa).NotEmpty().NotNull();
        }
    }
}