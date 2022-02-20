namespace Notes.Core.Contracts
{
    /// <summary>
    /// Запрос на регистрацию пользователя
    /// </summary>
    public class UserUpsertDto
    {
        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Name { get; set; }
    }
}
