using MagazynManager.Application.DataProviders;
using MagazynManager.Infrastructure.InputModel.Authentication;
using MediatR;

namespace MagazynManager.Application.Commands.Authentication
{
    public class RegisterCommand : IRequest<RegisterResult>
    {
        public RegisterModel RegisterModel { get; }

        public RegisterCommand(RegisterModel inputModel)
        {
            RegisterModel = inputModel;
        }
    }
}