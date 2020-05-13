using MagazynManager.Infrastructure.InputModel.Authentication;
using System;

namespace MagazynManager.Tests.ObjectMothers
{
    public static class LoginObjectMother
    {
        public static UserLoginModel GetAdminLoginModel()
        {
            return new UserLoginModel
            {
                Email = "admin@admin.com",
                Password = "admin",
                PrzedsiebiorstwoId = Guid.Parse("cf5cbb85-dcb0-470d-b8b9-9a29a097a73d")
            };
        }

        public static object GetBadLoginModel()
        {
            return new UserLoginModel
            {
                Email = "admin@admin.com",
                Password = "admin1",
                PrzedsiebiorstwoId = Guid.Parse("cf5cbb85-dcb0-470d-b8b9-9a29a097a73d")
            };
        }
    }
}