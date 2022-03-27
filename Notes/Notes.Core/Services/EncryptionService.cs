using Notes.Core.Interfaces;

namespace Notes.Core.Services
{
    /// <summary>
    /// Сервис шифрования паролей
    /// </summary>
    public class EncryptionService : IEncryptionService
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool ValidatePassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
