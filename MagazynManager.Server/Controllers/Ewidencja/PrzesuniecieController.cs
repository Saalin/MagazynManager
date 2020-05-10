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
    public class PrzesuniecieController : BaseController
    {
        public PrzesuniecieController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Pobiera listę dokumentów przesuniecia w danym przedsiębiorstwie
        /// </summary>
        /// <response code="200">Lista dokumentów przesunięcia</response>
        /// <response code="401">Błędne dane uwierzytelniające</response>
        [ProducesResponseType(typeof(List<Dokument>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet, Route("List")]
        [AuthorizePermission(AppArea.Ewidencjonowanie, Access.List)]
        public async Task<IActionResult> Get()
        {
            var list = await Mediator.Send(new DokumentPrzesunieciaListQuery(PrzedsiebiorstwoId));
            return Ok(list);
        }

        /// <summary>
        /// Przesuwa towar z magazynu na inny magazyn.
        /// Jeśli danego towaru nigdy nie było na magazynie przyjmującym jest on tworzony
        /// </summary>
        /// <param name="createModel">Model dokumentu wydania</param>
        /// <response code="200">Id nowoutworzonego dokumentu przesunięcia (ujemnego)</response>
        /// <response code="401">Błędne dane uwierzytelniające</response>
        /// <response code="400">Błąd biznesowy np. brak wystarczającej ilości towaru na magazynie</response>
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost, Route("Przesun")]
        [AuthorizePermission(AppArea.Ewidencjonowanie, Access.Create)]
        public async Task<IActionResult> Przesun(PrzesuniecieCreateModel createModel)
        {
            var id = await Mediator.Send(new PrzesunCommand(PrzedsiebiorstwoId, createModel));
            return Ok(id);
        }
    }
}