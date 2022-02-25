namespace Notes.Core.Contracts
{
    /// <summary>
    /// Данные для создания и обновления пользователя
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
        /// Имя
        /// </summary>
        public string Name { get; set; }
    }
}
