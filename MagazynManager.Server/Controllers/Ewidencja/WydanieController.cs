using MagazynManager.Application.Commands.Ewidencja;
using MagazynManager.Application.Queries.Ewidencja;
using MagazynManager.Domain.Entities.Dokumenty;
using MagazynManager.Infrastructure.Authorization;
using MagazynManager.Infrastructure.InputModel.Ewidencja;
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
    public class WydanieController : BaseController
    {
        public WydanieController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Pobiera listę dokumentów wydania w danym przedsiębiorstwie
        /// </summary>
        /// <response code="200">Lista dokumentów wydania</response>
        /// <response code="401">Błędne dane uwierzytelniające</response>
        [ProducesResponseType(typeof(List<Dokument>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [AuthorizePermission(AppArea.Ewidencjonowanie, Access.List)]
        [HttpGet, Route("List")]
        public async Task<IActionResult> Get()
        {
            var list = await Mediator.Send(new DokumentWydaniaListQuery(PrzedsiebiorstwoId));
            return Ok(list);
        }

        /// <summary>
        /// Wydaje towar z magazynu
        /// </summary>
        /// <param name="model"></param>
        /// <response code="200">Id nowoutworzonego dokumentu wydania</response>
        /// <response code="401">Błędne dane uwierzytelniające</response>
        /// <response code="400">Błąd biznesowy np. brak wystarczającej ilości towaru na magazynie</response>
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AuthorizePermission(AppArea.Ewidencjonowanie, Access.Create)]
        [HttpPost, Route("Wydaj")]
        public async Task<IActionResult> Wydaj(WydanieCreateModel model)
        {
            var id = await Mediator.Send(new WydajCommand(PrzedsiebiorstwoId, model));
            return Ok(id);
        }
    }
}