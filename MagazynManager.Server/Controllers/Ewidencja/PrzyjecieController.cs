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
    public class PrzyjecieController : BaseController
    {
        public PrzyjecieController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Pobiera listę dokumentów przyjęcia w danym przedsiębiorstwie
        /// </summary>
        /// <response code="200">Lista dokumentów przyjęcia</response>
        /// <response code="401">Błędne dane uwierzytelniające</response>
        [ProducesResponseType(typeof(List<Dokument>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [AuthorizePermission(AppArea.Ewidencjonowanie, Access.List)]
        [HttpGet, Route("List")]
        public async Task<IActionResult> Get()
        {
            var list = await Mediator.Send(new DokumentPrzyjeciaListQuery(PrzedsiebiorstwoId));
            return Ok(list);
        }

        /// <summary>
        /// Przyjmuje towary na magazyn
        /// </summary>
        /// <param name="model">Model dokumentu przyjęcia</param>
        /// <response code="200">Id nowoutworzonego dokumentu przyjęcia</response>
        /// <response code="401">Błędne dane uwierzytelniające</response>
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [AuthorizePermission(AppArea.Ewidencjonowanie, Access.Create)]
        [HttpPost, Route("Przyjmij")]
        public async Task<IActionResult> Przyjmij(PrzyjecieCreateModel model)
        {
            var id = await Mediator.Send(new PrzyjmijCommand(PrzedsiebiorstwoId, model));
            return Ok(id);
        }
    }
}