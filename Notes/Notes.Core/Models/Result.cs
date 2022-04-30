namespace Notes.Core.Models
{
    /// <summary>
    /// Результат с данными
    /// </summary>
    public class Result<TData> : Result
    {
        /// <summary>
        /// Данные
        /// </summary>
        public TData Data { get; set; }

        /// <summary>
        /// Создаёт новый экзмепляр класса <see cref="Result{TData}"/> с данными
        /// </summary>
        /// <param name="data"></param>
        public Result(TData data)
        {
            Data = data;
        }
    }

    /// <summary>
    /// Результат
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Маркер успешного выполнения
        /// </summary>
        public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);

        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Создаёт новый экземпляр класса <see cref="Result{TData}"/>
        /// </summary>
        public Result()
        { }
    }
}
