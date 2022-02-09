namespace Notes.Core.Contracts
{
    /// <summary>
    /// Результат аутентификации
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// Результат
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Создаёт новый экземпляр класса <seealso cref="LoginResponse"/>
        /// </summary>
        public LoginResponse()
        {
            IsSuccess = true;
        }

        /// <summary>
        /// Создаёт новый экземпляр класса <seealso cref="LoginResponse"/> с сообщением
        /// </summary>
        /// <param name="message">Сообщение</param>
        public LoginResponse(string message)
        {
            Message = message;
            IsSuccess = false;
        }
    }
}
