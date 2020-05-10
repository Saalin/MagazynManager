using System;

namespace MagazynManager.Application.DataProviders
{
    public class RegisterResult
    {
        public Guid UserId { get; set; }
        public AuthResult AuthResult { get; set; }
    }
}