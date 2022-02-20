namespace Notes.Core.Contracts
{
    /// <summary>
    /// Данные пользователя
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Захешированный пароль
        /// </summary>
        public string PasswordHash { get; set; }
    }
}
