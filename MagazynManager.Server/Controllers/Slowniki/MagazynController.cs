using MagazynManager.Application.Commands.Slowniki;
using MagazynManager.Application.Queries.Slowniki;
using MagazynManager.Domain.Entities.StukturaOrganizacyjna;
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
    public class MagazynController : BaseController
    {
        public MagazynController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Pobiera listę magazynów w przedsiębiorstwie
        /// </summary>
        /// <response code="200">Lista magazynów</response>
        /// <response code="401">Błędne dane uwierzytelniające</response>
        [ProducesResponseType(typeof(List<Magazyn>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet, Route("List")]
        [AuthorizePermission(AppArea.Slowniki, Access.List)]
        public async Task<IActionResult> Get()
        {
            var result = await Mediator.Send(new MagazynListQuery(PrzedsiebiorstwoId));
            return Ok(result);
        }

        /// <summary>
        /// Tworzy nowy magazyn
        /// </summary>
        /// <param name="model">Model magazynu</param>
        /// <response code="201">Id utworzonego elementu</response>
        /// <response code="401">Błędne dane uwierzytelniające</response>
        /// <response code="400">Błąd biznesowy np. próba utworzenia magazynu o takiej nazwie jak istniejący</response>
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        [AuthorizePermission(AppArea.Slowniki, Access.Create)]
        public async Task<IActionResult> Create(MagazynCreateModel model)
        {
            var result = await Mediator.Send(new MagazynCreateCommand(model.Skrot, model.Nazwa, PrzedsiebiorstwoId));
            return Ok(result);
        }

        /// <summary>
        /// Usuwa magazyn
        /// </summary>
        /// <param name="id">Id magazynu</param>
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
            await Mediator.Send(new MagazynDeleteCommand(id, PrzedsiebiorstwoId));
            return NoContent();
        }
    }
}