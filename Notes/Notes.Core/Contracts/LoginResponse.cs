namespace Notes.Core.Contracts
{
    /// <summary>
    /// Результат аутентификации
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Java Web Token
        /// </summary>
        public string Token { get; set; }
    }
}
