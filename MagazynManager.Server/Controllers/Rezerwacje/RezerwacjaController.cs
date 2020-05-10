using MagazynManager.Application.Commands.Rezerwacje;
using MagazynManager.Application.Queries.Rezerwacje;
using MagazynManager.Domain.Entities.Rezerwacje;
using MagazynManager.Infrastructure.Authorization;
using MagazynManager.Infrastructure.Dto.Rezerwacje;
using MagazynManager.Infrastructure.InputModel.Rezerwacje;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MagazynManager.Server.Controllers.Rezerwacje
{
    [ApiController]
    [Authorize, Route("[controller]")]
    public class RezerwacjaController : BaseController
    {
        public RezerwacjaController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Pobiera listę rezerwacji
        /// </summary>
        /// <response code="200">Lista dokumentów przyjęcia</response>
        /// <response code="401">Błędne dane uwierzytelniające</response>
        [ProducesResponseType(typeof(List<RezerwacjaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet, Route("List")]
        [AuthorizePermission(AppArea.Rezerwacje, Access.List)]
        public async Task<IActionResult> Get()
        {
            var list = await Mediator.Send(new RezerwacjaListQuery(PrzedsiebiorstwoId));
            return Ok(list);
        }

        /// <summary>
        /// Rezerwuje towar na magazynie
        /// </summary>
        /// <response code="200">Id utworzonej rezerwacji</response>
        /// <response code="401">Błędne dane uwierzytelniające</response>
        [ProducesResponseType(typeof(List<Rezerwacja>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost, Route("Rezerwuj")]
        [AuthorizePermission(AppArea.Rezerwacje, Access.Create)]
        public async Task<IActionResult> Rezerwuj(RezerwacjaCreateModel model)
        {
            var id = await Mediator.Send(new RezerwujCommand(PrzedsiebiorstwoId, UserId, model));
            return Ok(id);
        }

        /// <summary>
        /// Anuluje rezerwację
        /// </summary>
        /// <response code="204">Anulowano rezerwację</response>
        /// <response code="401">Błędne dane uwierzytelniające</response>
        /// <response code="400">Błąd biznesowy np. brak rezerwacji o takim id lub rezerwacja powiązana z dokumentem wydania</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete, Route("Anuluj/{id}")]
        [AuthorizePermission(AppArea.Rezerwacje, Access.Delete)]
        public async Task<IActionResult> Anuluj(Guid id)
        {
            await Mediator.Send(new AnulujRezerwacjeCommand(PrzedsiebiorstwoId, id, UserId));
            return NoContent();
        }

        /// <summary>
        /// Generuje dokument wydania na podstawie wybranej rezerwacji
        /// </summary>
        /// <response code="200">Id dokumentu wydania</response>
        /// <response code="401">Błędne dane uwierzytelniające</response>
        /// <response code="400">Błąd biznesowy np. próba ponownego wygenerowania wydania</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet, Route("GenerujWydanie/{id}")]
        [AuthorizePermission(AppArea.Rezerwacje, Access.Update)]
        public async Task<IActionResult> GenerujDokumentWydania(Guid rezerwacjaId)
        {
            var id = await Mediator.Send(new GenerujDokumentWydaniaCommand(PrzedsiebiorstwoId, rezerwacjaId));
            return Ok(id);
        }

        /// <summary>
        /// Usuwa przedawnine rezerwacje z przedsiębiorstwa
        /// </summary>
        /// <remarks>
        /// Wymaga uprawnień Rezerwacje/Admin
        /// </remarks>
        /// <response code="204">Usunięto przedawnione rezerwacje</response>
        /// <response code="401">Błędne dane uwierzytelniające</response>
        /// <response code="400">Błąd biznesowy np. brak rezerwacji o takim id</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete, Route("UsunPrzedawnione")]
        [AuthorizePermission(AppArea.Rezerwacje, Access.Admin)]
        public async Task<IActionResult> UsunPrzedawnione()
        {
            await Mediator.Send(new UsunPrzedawnioneRezerwacjeCommand(PrzedsiebiorstwoId));
            return NoContent();
        }
    }
}