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
        /// Выполнить аутентификацию
        /// </summary>
        /// <param name="request">Запрос на аутентификацию</param>
        /// <returns>Ответ на запрос аутентификации, содержащий JWT</returns>
        Task<LoginResponse> LoginAsync(LoginRequest request);

        /// <summary>
        /// Зарегистрировать нового пользователя в системе
        /// </summary>
        /// <param name="request">Запрос на регистрацию</param>
        /// <returns>Ответ на запрос регистрации, содержащий данные созданного пользователя</returns>
        Task<RegistrationResponse> RegisterAsync(RegistrationRequest request);
    }
}
