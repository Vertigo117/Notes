namespace Notes.Core.Contracts
{
    /// <summary>
    /// Результат регистрации пользователя
    /// </summary>
    public class RegistrationResponse
    {
        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Захешированный пароль
        /// </summary>
        public string PasswordHash { get; set; }
    }
}
