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
    public class KategoriaController : BaseController
    {
        public KategoriaController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Pobiera list� dost�pnych kategorii produkt�w
        /// </summary>
        /// <response code="200">Lista kategorii</response>
        /// <response code="401">B��dne dane uwierzytelniaj�ce</response>
        [ProducesResponseType(typeof(List<KategoriaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet, Route("List")]
        [AuthorizePermission(AppArea.Slowniki, Access.List)]
        public async Task<IActionResult> Get()
        {
            var result = await Mediator.Send(new KategoriaListQuery(PrzedsiebiorstwoId));
            return Ok(result);
        }

        /// <summary>
        /// Tworzy kategori� produtkow�
        /// </summary>
        /// <response code="201">Id utworzonego elementu</response>
        /// <response code="401">B��dne dane uwierzytelniaj�ce</response>
        /// <response code="400">B��d biznesowy np. pr�ba utworzenia kategorii o takiej nazwie jak istniej�ca</response>
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        [AuthorizePermission(AppArea.Slowniki, Access.Create)]
        public async Task<IActionResult> Create(KategoriaCreateModel inputModel)
        {
            var result = await Mediator.Send(new KategoriaCreateCommand(inputModel, PrzedsiebiorstwoId));
            return Ok(result);
        }

        /// <summary>
        /// Usuwa kategori� produtkow�
        /// </summary>
        /// <response code="204">Usuni�to element</response>
        /// <response code="401">B��dne dane uwierzytelniaj�ce</response>
        /// <response code="400">B��d biznesowy np. pr�ba usuni�cia u�ywanego elementu</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete, Route("{id}")]
        [AuthorizePermission(AppArea.Slowniki, Access.Delete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await Mediator.Send(new KategoriaDeleteCommand(id));
            return NoContent();
        }
    }
}