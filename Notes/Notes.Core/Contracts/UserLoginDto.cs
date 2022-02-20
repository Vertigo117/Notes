namespace Notes.Core.Contracts
{
    /// <summary>
    /// Запрос на авторизацию пользователя в системе
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
