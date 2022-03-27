namespace Notes.Core.Tests.Models
{
    /// <summary>
    /// Заголовок токена
    /// </summary>
    internal class Header
    {
        /// <summary>
        /// Алгоритм хеширования
        /// </summary>
        public string Alg { get; set; }

        /// <summary>
        /// Тип токена
        /// </summary>
        public string Typ { get; set; }

        public override bool Equals(object obj)
        {
            var header = obj as Header;
            return Alg == header.Alg && Typ == header.Typ;
        }

        public override int GetHashCode()
        {
            return Alg.GetHashCode() + Typ.GetHashCode();
        }
    }
}
