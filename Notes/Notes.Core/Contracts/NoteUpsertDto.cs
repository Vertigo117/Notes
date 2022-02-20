namespace Notes.Core.Contracts
{
    /// <summary>
    /// Данные для создания и обновления заметки
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
