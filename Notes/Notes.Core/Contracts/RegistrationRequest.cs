namespace Notes.Core.Contracts
{
    /// <summary>
    /// Запрос на регистрацию пользователя
    /// </summary>
    public class RegistrationRequest
    {
        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }
    }
}
