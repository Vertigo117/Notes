namespace Notes.Core.Tests.Models
{
    /// <summary>
    /// Данные токена
    /// </summary>
    internal class Payload
    {
        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Unique_name { get; set; }

        /// <summary>
        /// Not valid before
        /// </summary>
        public string Nbf { get; set; }

        /// <summary>
        /// Время истечения
        /// </summary>
        public string Exp { get; set; }

        /// <summary>
        /// Время выпуска
        /// </summary>
        public string Iat { get; set; }

        public override bool Equals(object obj)
        {
            var payload = obj as Payload;
            return payload.Nbf == Nbf
                && payload.Email == Email
                && payload.Iat == Iat
                && payload.Exp == Exp
                && payload.Unique_name == Unique_name;
        }

        public override int GetHashCode()
        {
            return Email.GetHashCode()
                + Nbf.GetHashCode()
                + Iat.GetHashCode()
                + Exp.GetHashCode()
                + Unique_name.GetHashCode();
        }
    }
}
