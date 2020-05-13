using MagazynManager.Application.DataProviders;
using MagazynManager.Infrastructure.InputModel.Authentication;
using MediatR;
using System;

namespace MagazynManager.Application.Commands.Authentication
{
    public sealed class LoginCommand : IRequest<AuthResult>, IEquatable<LoginCommand>
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

        public bool Equals(LoginCommand other)
        {
            return other.Email == Email && other.Password == Password && other.PrzedsiebiorstwoId == PrzedsiebiorstwoId;
        }
    }
}