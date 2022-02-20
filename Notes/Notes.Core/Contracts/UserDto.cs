namespace Notes.Core.Contracts
{
    /// <summary>
    /// Результат регистрации пользователя
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Уникальный идентификатор пользователя
        /// </summary>
        public int Id { get; set; }

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
