using System;

namespace MagazynManager.Infrastructure.InputModel.Authentication
{
    public class UserLoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public Guid PrzedsiebiorstwoId { get; set; }
    }
}