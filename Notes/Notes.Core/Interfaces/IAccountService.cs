using Microsoft.AspNetCore.Http;
using Notes.Core.Contracts;
using System.Threading.Tasks;

namespace Notes.Core.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса аутентификации
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Выполнить аутентификацию пользователя
        /// </summary>
        /// <param name="request">Запрос на аутентификацию</param>
        /// <param name="httpContext">Контекст запроса</param>
        /// <returns>задача, которая содержит результат аутентификации</returns>
        Task<LoginResponse> LoginAsync(LoginRequest request, HttpContext httpContext);

        /// <summary>
        /// Зарегистрировать пользователя в системе
        /// </summary>
        /// <param name="request">Запрос на регистрацию</param>
        /// <returns>Задача, которая содержит результат регистрации</returns>
        Task<RegistrationResponse> Register(RegistrationRequest request);

        /// <summary>
        /// Выйти из системы
        /// </summary>
        /// <param name="httpContext">Контекст запроса</param>
        /// <returns>Результат выполнения асинхронной операции</returns>
        Task LogoutAsync(HttpContext httpContext);
    }
}
