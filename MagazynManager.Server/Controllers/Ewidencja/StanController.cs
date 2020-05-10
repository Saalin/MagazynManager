using MagazynManager.Application.Queries.Ewidencja;
using MagazynManager.Infrastructure.Authorization;
using MagazynManager.Infrastructure.Dto.Ewidencja;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MagazynManager.Server.Controllers.Ewidencja
{
    [ApiController]
    [Authorize, Route("[controller]")]
    public class StanController : BaseController
    {
        public StanController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Pobiera stany aktualne produktów dostępnych na wybranym magazynie
        /// </summary>
        /// <param name="magazynId">Id magazynu</param>
        /// <response code="200">Lista stanów magazynowych</response>
        /// <response code="401">Błędne dane uwierzytelniające</response>
        [ProducesResponseType(typeof(List<StanAktualnyDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet, Route("AktualneStanyList/{magazynId}")]
        [AuthorizePermission(AppArea.Ewidencjonowanie, Access.List)]
        public async Task<IActionResult> GetStany(Guid magazynId)
        {
            var list = await Mediator.Send(new StanAktualnyMagazynuQuery(magazynId, PrzedsiebiorstwoId));
            return Ok(list);
        }
    }
}