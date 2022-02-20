namespace Notes.Core.Contracts
{
    /// <summary>
    /// Запрос на создание записи
    /// </summary>
    public class NoteUpsertDto
    {
        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Текст
        /// </summary>
        public string Text { get; set; }
    }
}
