namespace Notes.Core.Contracts
{
    /// <summary>
    /// Ответ на запрос создания новой заметки
    /// </summary>
    public class CreateNoteResponse
    {
        /// <summary>
        /// Уникальный идентификатор заметки
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название заметки
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Текст заметки
        /// </summary>
        public string Text { get; set; }
    }
}
