namespace Notes.Core.Contracts
{
    /// <summary>
    /// Результат аутентификации
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// Java Web Token
        /// </summary>
        public string Token { get; set; }
    }
}
