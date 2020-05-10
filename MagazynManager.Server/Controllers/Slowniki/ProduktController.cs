using MagazynManager.Application.Commands.Slowniki;
using MagazynManager.Application.Queries.Slowniki;
using MagazynManager.Infrastructure.Authorization;
using MagazynManager.Infrastructure.Dto.Slowniki;
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
    public class ProduktController : BaseController
    {
        public ProduktController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Pobiera listę wszystkich produktów
        /// </summary>
        /// <response code="200">Lista produtków</response>
        /// <response code="401">Błędne dane uwierzytelniające</response>
        [ProducesResponseType(typeof(List<ProduktDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet, Route("List")]
        [AuthorizePermission(AppArea.Ewidencjonowanie, Access.List)]
        public async Task<IActionResult> Get()
        {
            var result = await Mediator.Send(new ProduktListQuery(PrzedsiebiorstwoId));
            return Ok(result);
        }

        /// <summary>
        /// Tworzy nowy produkt przypisany do magazynu
        /// Jeśli jednostka miary i kategoria o danej nazwie nie istnieją są tworzone
        /// </summary>
        /// <param name="model"></param>
        /// <response code="201">Id utworzonego elementu</response>
        /// <response code="401">Błędne dane uwierzytelniające</response>
        /// <response code="400">Błąd biznesowy np. próba utworzenia produktu o takiej nazwie jak istniejący</response>
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        [AuthorizePermission(AppArea.Slowniki, Access.Create)]
        public async Task<IActionResult> Create(ProduktCreateModel model)
        {
            var command = new ProduktCreateCommand(model.ShortName, model.Name, model.JednostkaMiary, model.Kategoria, model.MagazynId, PrzedsiebiorstwoId);
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Usuwa produkt o wybranym Id
        /// </summary>
        /// <param name="id">Id produktu</param>
        /// <response code="204">Usunięto element</response>
        /// <response code="401">Błędne dane uwierzytelniające</response>
        /// <response code="400">Błąd biznesowy np. próba usunięcia używanego elementu</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete, Route("{id}")]
        [AuthorizePermission(AppArea.Slowniki, Access.Delete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await Mediator.Send(new ProduktDeleteCommand(id, PrzedsiebiorstwoId));
            return NoContent();
        }
    }
}