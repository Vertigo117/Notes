using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notes.Api.Models;
using Notes.Core.Contracts;
using Notes.Core.Interfaces;
using Notes.Core.Models;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Notes.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        /// <summary>
        /// Выполнить аутентификацию пользователя в системе
        /// </summary>
        /// <param name="request">Запрос на аутентификацию</param>
        /// <returns>Результат аутентификации, содержащий JWT</returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(TokenDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Login([FromBody] UserLoginDto request)
        {
            Result result = await accountService.LoginAsync(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result);
        }

        /// <summary>
        /// Зарегистрировать пользователя в системе
        /// </summary>
        /// <param name="request">Запрос на регистрацию</param>
        /// <returns>Http статус-код</returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType((typeof(UserDto)), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserUpsertDto request)
        {
            Result result = await accountService.RegisterAsync(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result);
        }

        /// <summary>
        /// Удалить пользователя
        /// </summary>
        /// <param name="email">Адрес электронной почты пользователя</param>
        /// <returns>Http статус-код</returns>
        [HttpDelete("{email}")]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] string email)
        {
            await accountService.DeleteAsync(email);
            return Ok();
        }

        /// <summary>
        /// Получить всех пользователей системы
        /// </summary>
        /// <returns>Коллекцию пользователей</returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Get()
        {
            IEnumerable<UserDto> users = await accountService.GetAsync();
            return Ok(users);
        }
    }
}
