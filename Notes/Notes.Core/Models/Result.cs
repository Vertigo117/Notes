namespace Notes.Core.Models
{
    /// <summary>
    /// Результат работы сервиса
    /// </summary>
    public class Result<TData>
    {
        /// <summary>
        /// Маркер успешного выполнения
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Данные
        /// </summary>
        public TData Data { get; }

        /// <summary>
        /// Создаёт новый экземпляр класса <see cref="Result{TData}"/> с сообщением об ошибке
        /// </summary>
        /// <param name="message">Сообщение об ошибке</param>
        public Result(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Создаёт новый экзмепляр класса <see cref="Result{TData}"/> с данными
        /// </summary>
        /// <param name="data"></param>
        public Result(TData data)
        {
            Data = data;
            IsSuccess = true;
        }
    }
}
