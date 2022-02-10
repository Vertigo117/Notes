namespace Notes.Core.Contracts
{
    /// <summary>
    /// Запрос на обновление данных заметки
    /// </summary>
    public class UpdateNoteRequest
    {
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
