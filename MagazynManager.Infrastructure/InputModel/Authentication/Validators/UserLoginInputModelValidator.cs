using FluentValidation;

namespace MagazynManager.Infrastructure.InputModel.Authentication.Validators
{
    public class UserLoginInputModelValidator : AbstractValidator<UserLoginModel>
    {
        public UserLoginInputModelValidator()
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.PrzedsiebiorstwoId).NotEmpty();
        }
    }
}