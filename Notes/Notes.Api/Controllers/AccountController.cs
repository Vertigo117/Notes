using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notes.Core.Contracts;
using Notes.Core.Interfaces;
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
        /// Выполняет аутентификацию пользователя в системе
        /// </summary>
        /// <param name="request">Запрос на аутентификацию</param>
        /// <returns>Результат аутентификации, содержащий JWT</returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            LoginResponse response = await accountService.LoginAsync(request);
            
            if (response == null)
            {
                return BadRequest("Неверное имя пользователя или пароль");
            }
            else
            {
                return Ok(response);
            }
        }

        /// <summary>
        /// Регистрирует пользователя в системе
        /// </summary>
        /// <param name="request">Запрос на регистрацию</param>
        /// <returns>Http-статус код</returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType((typeof(RegistrationResponse)), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest request)
        {
            RegistrationResponse response = await accountService.RegisterAsync(request);

            if (response == null)
            {
                return BadRequest("Пользователь с таким адресом электронной почты уже существует");
            }
            
            return Ok(response);
        }
    }
}
