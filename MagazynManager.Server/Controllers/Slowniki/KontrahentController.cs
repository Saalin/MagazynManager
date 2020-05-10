using MagazynManager.Application.Commands.Slowniki;
using MagazynManager.Application.Queries.Slowniki;
using MagazynManager.Domain.Entities.Kontrahent;
using MagazynManager.Infrastructure.Authorization;
using MagazynManager.Infrastructure.InputModel.Slowniki;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MagazynManager.Server.Controllers.Slowniki
{
    [ApiController]
    [Authorize, Route("[controller]")]
    public class KontrahentController : BaseController
    {
        public KontrahentController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Pobiera listę kontrahentów
        /// </summary>
        /// <response code="200">Lista magazynów</response>
        /// <response code="401">Błędne dane uwierzytelniające</response>
        [ProducesResponseType(typeof(List<Kontrahent>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet, Route("List")]
        [AuthorizePermission(AppArea.Slowniki, Access.List)]
        public async Task<IActionResult> Get()
        {
            var list = await Mediator.Send(new KontrahentListQuery(PrzedsiebiorstwoId));
            return Ok(list);
        }

        /// <summary>
        /// Dodaje kontrahenta
        /// </summary>
        /// <response code="200">Id nowoutworzonego kontrahenta</response>
        /// <response code="401">Błędne dane uwierzytelniające</response>
        /// <response code="400">Błąd biznesowy np. próbowano utworzyć kontrahenta o tym samym NIP</response>
        [ProducesResponseType(typeof(List<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        [AuthorizePermission(AppArea.Slowniki, Access.Create)]
        public async Task<IActionResult> Create(KontrahentCreateModel model)
        {
            var id = await Mediator.Send(new KontrahentCreateCommand(PrzedsiebiorstwoId, model));
            return Ok(id);
        }

        /// <summary>
        /// Usuwa kontrahenta
        /// </summary>
        /// <response code="204">Usunięto kontrahenta</response>
        /// <response code="401">Błędne dane uwierzytelniające</response>
        /// <response code="400">Błąd biznesowy np. próbowano usunąć nieistniejącego kontrahenta</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete, Route("{id}")]
        [AuthorizePermission(AppArea.Slowniki, Access.Delete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await Mediator.Send(new KontrahentDeleteCommand(PrzedsiebiorstwoId, id));
            return NoContent();
        }
    }
}