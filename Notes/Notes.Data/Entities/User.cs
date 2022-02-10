using System.Collections.Generic;

namespace Notes.Data.Entities
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class User
    {
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

        /// <summary>
        /// Заметки
        /// </summary>
        public ICollection<Note> Notes { get; set; }
    }
}
