using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Core.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса шифрования паролей
    /// </summary>
    public interface IEncryptionService
    {
        /// <summary>
        /// Проверить подлинность пароля
        /// </summary>
        /// <param name="password">Пароль</param>
        /// <param name="passwordHash">Хэш пароля</param>
        /// <returns></returns>
        bool ValidatePassword(string password, string passwordHash);

        /// <summary>
        /// Сгенерировать хеш пароля
        /// </summary>
        /// <param name="password">Пароль</param>
        /// <returns>Хеш пароля</returns>
        string HashPassword(string password);
    }
}
