using Notes.Core.Contracts;

namespace Notes.Core.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса генерации java web token'ов
    /// </summary>
    public interface IJwtService
    {
        /// <summary>
        /// Сгенерировать Java Web Token для пользователя, в качестве клэйма будет использован
        /// адрес электронной почты
        /// </summary>
        /// <param name="email">Адрес электронной почты пользователя</param>
        /// <returns>Строка с токеном</returns>
        string Generate(string email);
    }
}
