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
    public class JednostkaMiaryController : BaseController
    {
        public JednostkaMiaryController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Pobiera listę dostępnych w przedsiębiorstwie jednostek miar
        /// </summary>
        /// <response code="200">Lista jednostek miary</response>
        /// <response code="401">Błędne dane uwierzytelniające</response>
        [ProducesResponseType(typeof(List<JednostkaMiaryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet, Route("List")]
        [AuthorizePermission(AppArea.Slowniki, Access.List)]
        public async Task<IActionResult> Get()
        {
            var result = await Mediator.Send(new JednostkaMiaryListQuery(PrzedsiebiorstwoId));
            return Ok(result);
        }

        /// <summary>
        /// Dodaje jednostkę miary do przedsiębiorstwa
        /// </summary>
        /// <response code="201">Id utworzonego elementu</response>
        /// <response code="401">Błędne dane uwierzytelniające</response>
        /// <response code="400">Błąd biznesowy np. próba utworzenia jednostki miary o takiej nazwie jak istniejąca</response>
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        [AuthorizePermission(AppArea.Slowniki, Access.Create)]
        public async Task<IActionResult> Create([FromBody]JednostkaMiaryCreateModel model)
        {
            var result = await Mediator.Send(new JednostkaMiaryCreateCommand(PrzedsiebiorstwoId, model.Nazwa));
            return Created(nameof(Create), result);
        }

        /// <summary>
        /// Usuwa jednostkę miary
        /// </summary>
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
            await Mediator.Send(new JednostkaMiaryDeleteCommand(id, PrzedsiebiorstwoId));
            return NoContent();
        }
    }
}