using FluentValidation;

namespace MagazynManager.Infrastructure.InputModel.Authentication.Validators
{
    public class SetPermissionsModelValidator : AbstractValidator<SetPermissionsModel>
    {
        public SetPermissionsModelValidator()
        {
            RuleFor(x => x.Claims).NotNull();
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}