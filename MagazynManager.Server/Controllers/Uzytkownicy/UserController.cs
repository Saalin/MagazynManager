using MagazynManager.Application.Commands.Authentication;
using MagazynManager.Application.DataProviders;
using MagazynManager.Infrastructure.Authorization;
using MagazynManager.Infrastructure.InputModel.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MagazynManager.Server.Controllers.Uzytkownicy
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : BaseController
    {
        public UserController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Logowanie u�ytkownika do wybranego przedsi�biorstwa.
        /// </summary>
        /// <remarks>
        /// U�ytkownik pracuje w Magazynie zawsze w kontek�cie wybranego Przedsi�biorstwa.
        /// Po podaniu poprawnych danych uwierzytelniaj�cych dostajemy token JWT zawieraj�cy w sobie
        /// wszystkie po�wiadczenia potrzebne do okre�lnia poziomu autoryzacji u�ytkownika.
        /// Je�li u�ytkownik chce pracowa� w wielu Przedsi�biorstwach musi wygenerowa� osobne tokeny.
        ///
        /// Token wygasa po pewnym czasie, domy�lnie po 15 minutach.
        ///
        /// Token zawiera informacje potrzebne do jego ewentualnego od�wie�enia.
        /// </remarks>
        /// <param name="loginUserDto">Model danych logowania</param>
        /// <returns>Token autoryzuj�cy</returns>
        /// <response code="200">Zwraca token autoryzuj�cy loguj�cego si� u�ytkownika</response>
        /// <response code="401">B��dne dane uwierzytelniaj�ce</response>
        [AllowAnonymous]
        [HttpPost("Login")]
        [ProducesResponseType(typeof(AuthResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] UserLoginModel loginUserDto)
        {
            var result = await Mediator.Send(new LoginCommand(loginUserDto));
            return result == null ? (IActionResult)Unauthorized() : Ok(result);
        }

        /// <summary>
        /// Rejestruje u�ytkownika
        /// </summary>
        /// <remarks>
        /// Rejestruje u�ytkownika nie nadaj�c mu �adnych uprawnie�.
        /// </remarks>
        /// <param name="registerInputModel"></param>
        /// <returns>Token autoryzuj�cy</returns>
        /// <response code="200">Zwraca id i token autoryzuj�cy nowo utworzonego u�ytkownika</response>
        /// <response code="401">Wyst�pi� b��d przy rejestracji</response>
        [AllowAnonymous]
        [HttpPost("Register")]
        [ProducesResponseType(typeof((Guid, AuthResult)), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerInputModel)
        {
            var result = await Mediator.Send(new RegisterCommand(registerInputModel));
            return result == null ? (IActionResult)Unauthorized() : Ok(result);
        }

        /// <summary>
        /// Od�wie�a token uwierzytelniaj�cy na podstawie wcze�niej wystawionego tokena
        /// </summary>
        /// <param name="credentials">Token otrzymany przy zalgowaniu</param>
        /// <returns>Od�wie�ony token</returns>
        /// <response code="200">Nowy token autoryzuj�cy</response>
        /// <response code="401">Niepoprawny b�d� przestarza�y token od�wie�aj�cy</response>
        [HttpPost("Refresh")]
        [ProducesResponseType(typeof(AuthResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Refresh([FromBody] AuthResult credentials)
        {
            var result = await Mediator.Send(new RefreshTokenCommand(credentials));
            if (result == null)
            {
                return Unauthorized();
            }
            return Ok(result);
        }

        /// <summary>
        /// Nadaje uprawnienia u�ytkownikowi o wybranym id
        /// </summary>
        /// <remarks>
        /// Ustawia wszystkie uprawnienia na takie jakie zosta�y wys�ane.
        /// Wymaga poziomu autoryzacji Administracja/Admin
        ///
        /// Format po�wiadcze� - klucz: Permission.[Administracja/Slowniki/Ewidencjonowanie/Rezerwacje], warto��: dowolny podci�g "lrcuda",
        /// gdzie l - list, r - read, c - create, u - update, d - delete, a - admin
        /// </remarks>
        /// <response code="204">Zmieniono uprawnienia</response>
        /// <response code="401">Niepoprawny b�d� przestarza�y token od�wie�aj�cy</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("NadajUprawnienia")]
        [AuthorizePermission(AppArea.Administracja, Access.Admin)]
        public async Task<IActionResult> NadajUprawnienia(SetPermissionsModel model)
        {
            await Mediator.Send(new SetPermissionsCommand(PrzedsiebiorstwoId, model));
            return NoContent();
        }
    }
}