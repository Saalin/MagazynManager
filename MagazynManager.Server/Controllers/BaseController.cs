using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace MagazynManager.Server.Controllers
{
    [Produces("application/json")]
    public abstract class BaseController : ControllerBase
    {
        protected readonly IMediator Mediator;

        protected BaseController(IMediator mediator)
        {
            Mediator = mediator;
        }

        protected Guid PrzedsiebiorstwoId
        {
            get
            {
                var id = User.Claims.Where(c => c.Type == "PrzedsiebiorstwoId")
                    .Select(c => c.Value).SingleOrDefault();

                return Guid.Parse(id);
            }
        }

        protected Guid UserId
        {
            get
            {
                var id = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                    .Select(c => c.Value).SingleOrDefault();

                return Guid.Parse(id);
            }
        }
    }
}