namespace Notes.Data.Entities
{
    /// <summary>
    /// Заметка
    /// </summary>
    public class Note
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Текст
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public User User { get; set; }
    }
}
