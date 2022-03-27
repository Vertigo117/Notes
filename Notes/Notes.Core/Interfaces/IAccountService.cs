using Notes.Core.Contracts;
using Notes.Core.Models;
using System.Collections.Generic;
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
        /// <param name="userLoginDto">Данные для авторизации</param>
        /// <returns>Результат авторизации</returns>
        Task<Result<TokenDto>> LoginAsync(UserLoginDto userLoginDto);

        /// <summary>
        /// Зарегистрировать нового пользователя в системе
        /// </summary>
        /// <param name="userUpsertDto">Данные для регистрации</param>
        /// <returns>Результат регистрации</returns>
        Task<Result<UserDto>> RegisterAsync(UserUpsertDto userUpsertDto);

        /// <summary>
        /// Удалить пользователя с указанным адресом электронной почты
        /// </summary>
        /// <param name="email">Адрес электронной почты пользователя</param>
        /// <returns>Задача, которая содержит результат выполнения асинхронной операции</returns>
        Task DeleteAsync(string email);

        /// <summary>
        /// Получить всех пользователей
        /// </summary>
        /// <returns>Коллекцию пользователей</returns>
        Task<IEnumerable<UserDto>> GetAsync();
    }
}
