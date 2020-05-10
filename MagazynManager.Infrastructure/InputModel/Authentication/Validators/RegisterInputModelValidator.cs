using FluentValidation;

namespace MagazynManager.Infrastructure.InputModel.Authentication.Validators
{
    public class RegisterInputModelValidator : AbstractValidator<RegisterModel>
    {
        public RegisterInputModelValidator()
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
            RuleFor(x => x.Password).MinimumLength(4);
            RuleFor(x => x.Age).GreaterThan(13);
        }
    }
}