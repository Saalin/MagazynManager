using MagazynManager.Application.DataProviders;
using MagazynManager.Infrastructure.InputModel.Authentication;
using MediatR;
using System;

namespace MagazynManager.Application.Commands.Authentication
{
    public class LoginCommand : IRequest<AuthResult>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public Guid PrzedsiebiorstwoId { get; set; }

        public LoginCommand(UserLoginModel inputModel)
        {
            Email = inputModel.Email;
            Password = inputModel.Password;
            PrzedsiebiorstwoId = inputModel.PrzedsiebiorstwoId;
        }
    }
}