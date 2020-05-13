using MagazynManager.Application.CommandHandlers.Authentication;
using MagazynManager.Application.Commands.Authentication;
using MagazynManager.Domain.Entities.Uzytkownicy;
using MagazynManager.Domain.Specification.Specifications;
using MagazynManager.Infrastructure.InputModel.Authentication;
using MediatR;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MagazynManager.Tests.UnitTests.Authentication
{
    [Category("UnitTest")]
    [TestFixture]
    public class AuthTest : UnitTest
    {
        [Test]
        public async Task RegisterTest()
        {
            var userRepository = new InMemoryUserRepository();
            var mediator = Substitute.For<IMediator>();
            var registerCommandHandler = new RegisterHandler(userRepository, mediator);

            await registerCommandHandler.Handle(new RegisterCommand(new RegisterModel
            {
                Email = "test@test.com",
                Age = 22,
                Password = "abc123"
            }), new CancellationToken());

            await mediator.Received().Send(Arg.Is<LoginCommand>(x => x.Email == "test@test.com" && x.Password == "abc123"));
        }

        [Test]
        public async Task SetPermissionTest()
        {
            var userRepository = new InMemoryUserRepository();
            var setPermissionCommandHandler = new SetPermissionsCommandHandler(userRepository);

            var adminId = Guid.Parse("3A7C2E38-385E-4C6A-8BFA-1767A3EBCCCC");

            await setPermissionCommandHandler.Handle(new SetPermissionsCommand(PrzedsiebiorstwoId, new SetPermissionsModel
            {
                UserId = adminId,
                Claims = new List<ClaimModel>
                {
                    new ClaimModel
                    {
                        Name = "Permission.Slowniki",
                        Value = "l"
                    }
                }
            }), new CancellationToken());

            var user = userRepository.GetUser(new IdSpecification<User, Guid>(adminId), PrzedsiebiorstwoId);

            Assert.That(user.Claims.Count(), Is.EqualTo(1));
        }
    }
}