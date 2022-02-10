namespace Notes.Api.Models
{
    /// <summary>
    /// Ошибка приложения
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Стек вызовов
        /// </summary>
        public string StackTrace { get; set; }
    }
}
