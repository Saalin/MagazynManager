using FluentValidation;

namespace MagazynManager.Infrastructure.InputModel.Authentication.Validators
{
    public class ClaimModelValidator : AbstractValidator<ClaimModel>
    {
        public ClaimModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Value).NotEmpty();
        }
    }
}