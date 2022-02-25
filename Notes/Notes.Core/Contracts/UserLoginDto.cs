namespace Notes.Core.Contracts
{
    /// <summary>
    /// Данные пользователя для авторизации в системе
    /// </summary>
    public class UserLoginDto
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
